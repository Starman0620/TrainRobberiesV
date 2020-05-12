﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using Newtonsoft.Json;
using TrainRobberiesV.Items;
using UniversalInventorySystem;

namespace TrainRobberiesV
{
    public class Main : Script
    {
        private ModConfig config = new ModConfig();

        private List<Entity> robbedCars = new List<Entity>();

        public Main()
        {
            Mod.VerifyFiles();
            config = LoadConfig();
            Tick += OnTick;
            UI.Notify("~y~Train Robberies V ~w~started successfully!");
        }

        private void OnTick(object sender, EventArgs e)
        {
            Vehicle[] vehicles = World.GetNearbyVehicles(Game.Player.Character, 25.5f);
            foreach(Vehicle veh in vehicles)
            {
                FreightCar car = null;
                foreach (FreightCar fc in config.cars) if (new Model(fc.modelName) == veh.Model) car = fc;
                if (car != null && !robbedCars.Contains(veh) && veh.IsAlive)
                {
                    if (veh.HasBone(car.bone))
                    {
                        Vector3 rearPos = veh.GetBoneCoord(veh.GetBoneIndex(car.bone));
                        if (World.GetDistance(rearPos, Game.Player.Character.Position) <= car.radius)
                        {
                            // The player can rob the train
                            UI.ShowHelpMessage("Press ~y~E ~w~to rob the train", 1, true);

                            if (Game.IsControlJustPressed(0, GTA.Control.Talk))
                            {
                                // Fading effect when robbing
                                Game.FadeScreenOut(1500);
                                Wait(3000);
                                SearchTrain(veh);
                                Game.FadeScreenIn(1500);
                            }
                        }
                    }
                }
            }
        }

        private void SearchTrain(Vehicle train)
        {
            robbedCars.Add(train);
            Random r = new Random();

            // Get a random item and give it to the user
            var item = config.items[r.Next(0, config.items.Count)];
            UI.Notify($"Train looted: {item.itemName} (${item.itemValue})");
            Game.Player.Money += item.itemValue; // TODO: Replace this with Universal Inventory System stuff
        }

        private ModConfig LoadConfig()
        {
            // Read and deserialze the mod configuration
            string json = File.ReadAllText("scripts\\TrainRobberiesV.json");
            return JsonConvert.DeserializeObject<ModConfig>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }
    }
}
