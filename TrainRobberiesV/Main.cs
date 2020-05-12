﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;

namespace TrainRobberiesV
{
    public class Main : Script
    {
        private FreightCar car = new FreightCar() // Create a new FreightCar object to store the one lootable car type. This will be replaced by the dictionary below in the first actual release
        {
            //modelName = "freightcont1",
            modelName = "tankercar",
            radius = 2.5f
        };
        //private Dictionary<Model, FreightCar> cars = new Dictionary<Model, FreightCar>(); // This will likely be used once json loading is implemented to store the FreightCars for the rest of the script to use

        public Main()
        {
            // TODO: Add json loading here for the FreightCar list

            Tick += OnTick;
            KeyDown += OnKeyDown;
            UI.Notify("Train Robberies V started successfully!");
        }

        private void OnTick(object sender, EventArgs e)
        {
            Vehicle[] vehicles = World.GetNearbyVehicles(Game.Player.Character, 25.5f);
            foreach(Vehicle veh in vehicles)
            {
                // TODO: Instead of checking for the single car object, check for matches in the cars dictionary
                if(veh.Model == new Model(car.modelName))
                {
                    Vector3 rearPos = veh.GetBoneCoord(veh.GetBoneIndex("bogie_r"));
                    if (World.GetDistance(rearPos, Game.Player.Character.Position) <= car.radius)
                    {
                        // The player can rob the train
                        UI.ShowHelpMessage("Hold ~y~E ~w~to rob the train", 1, true);
                    }
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.E)
            {
                Vehicle[] vehicles = World.GetNearbyVehicles(Game.Player.Character, 25.5f);
                foreach (Vehicle veh in vehicles)
                {
                    // TODO: Instead of checking for the single car object, check for matches in the cars dictionary
                    if (veh.Model == new Model(car.modelName))
                    {
                        Vector3 rearPos = veh.GetBoneCoord(veh.GetBoneIndex("bogie_r"));
                        if (World.GetDistance(rearPos, Game.Player.Character.Position) <= car.radius)
                        {
                            // The player can rob the train
                            UI.ShowSubtitle("Robbed!");
                        }
                    }
                }
            }
        }
    }
}
