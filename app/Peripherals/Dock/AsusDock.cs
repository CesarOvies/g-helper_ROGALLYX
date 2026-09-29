using GHelper.USB;

namespace GHelper.Peripherals.Dock
{
    public class AsusDock : IPeripheral
    {
        public bool IsDeviceReady { get; set; } = true;

        public int Battery => -1;

        public bool Charging => false;

        public bool CanExport() => false;

        public byte[] Export() => Array.Empty<byte>();

        public bool Import(byte[] blob) => false;

        public PeripheralType DeviceType() => PeripheralType.Dock;

        public string GetDisplayName() => "ROG Bulwark Dock";

        public bool HasBattery() => false;

        public void SynchronizeDevice()
        {
            if (AppConfig.IsDockAuraSync())
            {
                int _speed = (Aura.Speed == AuraSpeed.Normal) ? 0xeb : (Aura.Speed == AuraSpeed.Fast) ? 0xf5 : 0xe1;
                Aura.ApplyBulwark(Aura.Mode, Aura.Color1, Aura.Color2, _speed, Aura.GetBrightness(), force: true);
            }
        }

        public void ReadBattery()
        {
        }
    }
}
