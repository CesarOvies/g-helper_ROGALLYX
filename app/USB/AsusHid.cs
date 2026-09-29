using HidSharp;
using HidSharp.Reports;
using System.Text;

namespace GHelper.USB;
public static class AsusHid
{
    public const int ASUS_ID = 0x0b05;

    public const byte INPUT_ID = 0x5a;
    public const byte AURA_ID = 0x5d;

    public const byte BULWARK_ID = 0xec;

    public static int[] MAIN_AURA_PIDS = { 0x1a30, 0x1854, 0x1869, 0x1866, 0x19b6, 0x1822, 0x1837, 0x1854, 0x184a, 0x183d, 0x8502, 0x1807, 0x17e0, 0x1abe, 0x1b4c, 0x1b6e, 0x1b2c, 0x8854, 0x1CE7, 0x1bf2, 0x1cd7, 0x1cd8 };
    public static int[] REAR_LIGHT_PIDS = { 0x18c6 };
    public static int[] BULWARK_PIDS = { 0x1c7f };
    public static int[] ALL_PIDS = MAIN_AURA_PIDS.Concat(REAR_LIGHT_PIDS).Concat(BULWARK_PIDS).ToArray();

    public static readonly object hidLock = new();

    static HidStream? auraStream;
    static int auraFeatLen;
    static byte[]? auraScratch;

    static void EnsureAuraStream()
    {
        if (auraStream != null) return;
        auraStream = FindHidStream(AURA_ID);
        if (auraStream == null) return;
        auraFeatLen = auraStream.Device.GetMaxFeatureReportLength();
        auraScratch = auraFeatLen > 0 ? new byte[auraFeatLen] : null;
    }

    static void DisposeAuraStream()
    {
        auraStream?.Dispose();
        auraStream = null;
        auraFeatLen = 0;
        auraScratch = null;
    }

    public static IEnumerable<HidDevice>? FindDevices(byte reportId, int[]? pids = null)
    {
        IEnumerable<HidDevice> deviceList;

        try
        {
            var allDevices = DeviceList.Local.GetHidDevices(ASUS_ID);
            var filteredDevices = new List<HidDevice>();

            foreach (var device in allDevices)
            {
                try
                {
                    if ((pids != null ? pids.Contains(device.ProductID) : ALL_PIDS.Contains(device.ProductID)) &&
                        device.CanOpen &&
                        (device.GetMaxFeatureReportLength() > 0 || device.GetMaxOutputReportLength() > 0))
                    {
                        filteredDevices.Add(device);
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"Error checking HID device {device.ProductID:X}: {ex.Message}");
                }
            }

            deviceList = filteredDevices;
        }
        catch (Exception ex)
        {
            Logger.WriteLine($"Error enumerating HID devices: {ex.Message}");
            yield break;
        }

        foreach (var device in deviceList)
        {
            bool isValid = false;
            try
            {
                var desc = device.GetReportDescriptor();
                isValid = desc.TryGetReport(ReportType.Feature, reportId, out _) ||
                          desc.TryGetReport(ReportType.Output, reportId, out _);
            }
            catch (Exception)
            {
                //Logger.WriteLine($"Error getting report descriptor for device {device.ProductID.ToString("X")}: {ex.Message}");
            }
            if (isValid) yield return device;
        }
    }

    public static HidStream? FindHidStream(byte reportId)
    {
        try
        {
            var devices = FindDevices(reportId);
            if (devices is null) return null;

            if (AppConfig.IsZ13())
            {
                var z13 = devices.Where(device => device.ProductID == 0x1a30).FirstOrDefault();
                if (z13 is not null) return z13.Open();
            }

            if (AppConfig.IsS17())
            {
                var s17 = devices.Where(device => device.ProductID == 0x18c6).FirstOrDefault();
                if (s17 is not null) return s17.Open();
            }

            if (AppConfig.IsDUO())
            {
                var duo = devices.Where(device => device.ProductID == 0x1cd7 || device.ProductID == 0x1cd8).FirstOrDefault();
                if (duo is not null) return duo.Open();
            }

            foreach (var device in devices)
                Logger.WriteLine($"Input available: {device.DevicePath} {device.ProductID.ToString("X")} {device.GetMaxFeatureReportLength()}");

            return devices.FirstOrDefault()?.Open();
        }
        catch (Exception ex)
        {
            Logger.WriteLine($"Error accessing HID device: {ex.Message}");
        }

        return null;
    }

    public static void WriteInput(byte[] data, string? log = "USB")
    {
        lock (hidLock)
        foreach (var device in FindDevices(INPUT_ID))
        {
            try
            {
                using (var stream = device.Open())
                {
                    var payload = new byte[device.GetMaxFeatureReportLength()];
                    Array.Copy(data, payload, data.Length);
                    stream.SetFeature(payload);
                    if (log is not null) Logger.WriteLine($"{log} {device.ProductID.ToString("X")}|{device.GetMaxFeatureReportLength()}: {BitConverter.ToString(data)}");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Error setting feature {device.GetMaxFeatureReportLength()} {device.DevicePath}: {BitConverter.ToString(data)} {ex.Message}");

            }
        }
    }

    public static void InitInput(string? log = "Input Init")
    {
        WriteInput([INPUT_ID, .. Encoding.ASCII.GetBytes("ASUS Tech.Inc.")], log);
    }

    public static void Write(byte[] data, string log = "USB")
    {
        Write(new List<byte[]> { data }, log);
    }

    public static void Write(List<byte[]> dataList, string log = "USB", int[]? pids = null)
    {
        var devices = FindDevices(AURA_ID, pids);
        if (devices is null) return;

        lock (hidLock)
        foreach (var device in devices)
            try
            {
                using (var stream = device.Open())
                    foreach (var data in dataList)
                        try
                        {
                            stream.Write(data);
                            if (log is not null) Logger.WriteLine($"{log} {device.ProductID.ToString("X")}: {BitConverter.ToString(data)}");
                        }
                        catch (Exception ex)
                        {
                            if (log is not null) Logger.WriteLine($"Error writing {log} {device.ProductID.ToString("X")}: {ex.Message} {BitConverter.ToString(data)} ");
                        }
            }
            catch (Exception ex)
            {
                if (log is not null) Logger.WriteLine($"Error opening {log} {device.ProductID:X4}: {ex.Message}");
            }
    }

    public static bool HasBulwark()
    {
        try
        {
            return DeviceList.Local.GetHidDevices(ASUS_ID).Any(d => BULWARK_PIDS.Contains(d.ProductID));
        }
        catch
        {
            return false;
        }
    }


    public static void WriteBulwark(List<byte[]> dataList, string log = "Bulwark")
    {
        var devices = FindDevices(BULWARK_ID, BULWARK_PIDS);
        if (devices is null) return;

        lock (hidLock)
        foreach (var device in devices)
            try
            {
                using (var stream = device.Open())
                {
                    int outLen = device.GetMaxOutputReportLength();
                    foreach (var data in dataList)
                        try
                        {
                            byte[] packet = data;
                            if (outLen > 0 && data.Length < outLen)
                            {
                                packet = new byte[outLen];
                                Array.Copy(data, packet, data.Length);
                            }
                            stream.Write(packet);
                            if (log is not null) Logger.WriteLine($"{log} {device.ProductID:X4}: {BitConverter.ToString(packet, 0, Math.Min(16, packet.Length))}");
                        }
                        catch (Exception ex)
                        {
                            if (log is not null) Logger.WriteLine($"Error writing {log} {device.ProductID:X4}: {ex.Message}");
                        }
                }
            }
            catch (Exception ex)
            {
                if (log is not null) Logger.WriteLine($"Error opening {log} {device.ProductID:X4}: {ex.Message}");
            }
    }

    public static void SetFeatureAura(byte[] data, bool retry = true)
    {
        EnsureAuraStream();
        if (auraStream == null)
        {
            Logger.WriteLine("Aura stream not found");
            return;
        }

        try
        {
            byte[] payload = data;
            if (auraScratch != null && data.Length < auraFeatLen)
            {
                Array.Clear(auraScratch, 0, auraFeatLen);
                Array.Copy(data, auraScratch, data.Length);
                payload = auraScratch;
            }
            lock (hidLock) auraStream.SetFeature(payload);
        }
        catch (Exception ex)
        {
            Logger.WriteLine($"Error setting feature on HID device: {ex.Message} {BitConverter.ToString(data, 0, Math.Min(16, data.Length))}");
            DisposeAuraStream();
            if (retry) SetFeatureAura(data, false);
        }
    }


    public static byte[]? AuraProbe(bool query, string log = "Aura Probe")
    {
        var device = FindDevices(AURA_ID)?.FirstOrDefault();
        if (device == null)
        {
            Logger.WriteLine($"{log}: no device");
            return null;
        }

        int featLen = device.GetMaxFeatureReportLength();

        byte[][] primers = [
            [AURA_ID, 0xB9],
            [AURA_ID, .. Encoding.ASCII.GetBytes("ASUS Tech.Inc.")],
        ];
        byte[] queryBytes = [AURA_ID, 0x05, 0x20, 0x31, 0x00, 0x20];

        try
        {
            using var stream = device.Open();

            foreach (var primer in primers)
                stream.Write(primer);
            stream.Write(queryBytes);

            if (!query) return null;

            var response = new byte[featLen];
            response[0] = AURA_ID;
            stream.GetFeature(response);

            for (int i = 0; i < 4; i++)
                if (response[i] != queryBytes[i]) return null;

            Logger.WriteLine($"{log}: {BitConverter.ToString(response)}");
            return response;
        }
        catch (Exception ex)
        {
            Logger.WriteLine($"{log} error: {ex.Message}");
            return null;
        }
    }

}

