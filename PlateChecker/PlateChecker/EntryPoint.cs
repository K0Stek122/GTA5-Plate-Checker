using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rage;

[assembly: Rage.Attributes.Plugin("PlateChecker", Description = "Lets you check Plates in front of you", Author = "K0Stek")]
namespace PlateChecker
{
    public static class EntryPoint
    {
        private static Vehicle GetVehicleInFront(float Distance)
        {
            Vector3 offSetPos = Game.LocalPlayer.Character.CurrentVehicle.GetOffsetPosition(Vector3.RelativeFront * Distance);
            Vehicle[] vehicleList = Game.LocalPlayer.Character.GetNearbyVehicles(16);
            foreach (Vehicle veh in vehicleList)
            {
                if (!veh.HasSiren && veh != Game.LocalPlayer.Character.CurrentVehicle)
                {
                    if (Vector3.Distance(offSetPos, veh.Position) < Distance)
                        return veh;
                }
            }
            return null;
        }

        public static void Main()
        {
            InitializationFile iniFile = new InitializationFile("plugins//PlateChecker.ini");

            //Keys key = iniFile.Read<Keys>("Keys", "ReadPlate", Keys.F3);
            Keys key = iniFile.ReadEnum<Keys>("Keys", "ReadPlate", Keys.F3);

            GameFiber.StartNew(delegate
            {
                while (true)
                {
                    GameFiber.Yield();
                    if (Game.IsKeyDown(key))
                    {
                        Vehicle closestVehicle = Game.LocalPlayer.Character.IsInAnyVehicle(false) ? GetVehicleInFront(7.5f) : GetVehicleInFront(4.2f);
                        if (closestVehicle != null)
                            Game.DisplayHelp("Plate: " + closestVehicle.LicensePlate.ToString());
                    }
                }
            });
        }
    }
}
