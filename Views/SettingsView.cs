﻿using Blish_HUD.Controls;
using Microsoft.Xna.Framework;
using Blish_HUD.Settings.UI.Views;
using Blish_HUD.Graphics.UI;
using System.Linq;

namespace Manlaan.Mounts.Views
{
    class SettingsView : View
    {
        protected override void Build(Container buildPanel) {
            int labelWidth                = 100;
            int orderWidth                = 80;
            int bindingWidth              = 150;
            int mountsAndRadialInputWidth = 125;

            Panel mountsLeftPanel = new Panel() {
                CanScroll = false,
                Parent = buildPanel,
                HeightSizingMode = SizingMode.AutoSize,
                Width = 400,
                Location = new Point(10, 10),
            };
            Panel otherPanel = new Panel() {
                CanScroll = false,
                Parent = buildPanel,
                HeightSizingMode = SizingMode.AutoSize,
                Width = 330,
                Location = new Point(mountsLeftPanel.Right + 20, 10),
            };
            Panel manualPanel = new Panel() {
                CanScroll = false,
                Parent = buildPanel,
                HeightSizingMode = SizingMode.AutoSize,
                Width = 330,
                Location = new Point(mountsLeftPanel.Right + 20, 93),
            };
            DisplayManualPanelIfNeeded(manualPanel);

            #region Mounts Panel
            Label keybindWarning_Label = new Label()
            {
                Location = new Point(5, 2),
                Width = 330,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = mountsLeftPanel,
                Text = "MOUNT KEYBINDS MUST MATCH INGAME KEYBINDS",
                TextColor = Color.Red,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            Label settingOrderLeft_Label = new Label() {
                Location = new Point(labelWidth + 5, keybindWarning_Label.Bottom + 6),
                Width = orderWidth,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = mountsLeftPanel,
                Text = "Order",
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            Label settingBindingLeft_Label = new Label() {
                Location = new Point(settingOrderLeft_Label.Right + 5, settingOrderLeft_Label.Top),
                Width = bindingWidth,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = mountsLeftPanel,
                Text = "Key Binding",
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            int curY = settingOrderLeft_Label.Bottom;

            foreach (var mount in Module._mounts)
            {
                Label settingMount_Label = new Label()
                {
                    Location = new Point(0, curY + 6),
                    Width = labelWidth,
                    AutoSizeHeight = false,
                    WrapText = false,
                    Parent = mountsLeftPanel,
                    Text = $"{mount.DisplayName}: ",
                };
                Dropdown settingMount_Select = new Dropdown()
                {
                    Location = new Point(settingMount_Label.Right + 5, settingMount_Label.Top - 4),
                    Width = orderWidth,
                    Parent = mountsLeftPanel,
                };
                foreach (int i in Module._mountOrder)
                {
                    if (i == 0)
                        settingMount_Select.Items.Add("Disabled");
                    else
                        settingMount_Select.Items.Add(i.ToString());
                }
                settingMount_Select.SelectedItem = !mount.IsAvailable ? "Disabled" : mount.OrderSetting.Value.ToString();
                settingMount_Select.ValueChanged += delegate {
                    if (settingMount_Select.SelectedItem.Equals("Disabled"))
                        mount.OrderSetting.Value = 0;
                    else
                        mount.OrderSetting.Value = int.Parse(settingMount_Select.SelectedItem);
                };
                KeybindingAssigner settingRaptor_Keybind = new KeybindingAssigner(mount.KeybindingSetting.Value)
                {
                    NameWidth = 0,
                    Size = new Point(bindingWidth, 20),
                    Parent = mountsLeftPanel,
                    Location = new Point(settingMount_Select.Right + 5, settingMount_Label.Top - 1),
                };
                settingRaptor_Keybind.BindingChanged += delegate {
                    mount.KeybindingSetting.Value = settingRaptor_Keybind.KeyBinding;                    
                };

                curY = settingMount_Label.Bottom;
            }


            int labelWidth2 = 200;
            Label settingDefaultSettingsMount_Label = new Label()
            {
                Location = new Point(0, curY + 24),
                Width = labelWidth2,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = mountsLeftPanel,
                Text = "Default mount settings: "
            };
            Label settingDefaultMount_Label = new Label()
            {
                Location = new Point(0, settingDefaultSettingsMount_Label.Bottom + 6),
                Width = labelWidth2,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = mountsLeftPanel,
                Text = "Default mount: ",
            };
            Dropdown settingDefaultMount_Select = new Dropdown()
            {
                Location = new Point(settingDefaultMount_Label.Right + 5, settingDefaultMount_Label.Top - 4),
                Width    = mountsAndRadialInputWidth,
                Parent   = mountsLeftPanel,
            };
            settingDefaultMount_Select.Items.Add("Disabled");
            var mountNames = Module._mounts.Select(m => m.Name);
            foreach (string i in mountNames)
            {
                settingDefaultMount_Select.Items.Add(i.ToString());
            }
            settingDefaultMount_Select.SelectedItem = mountNames.Any(m => m == Module._settingDefaultMountChoice.Value) ? Module._settingDefaultMountChoice.Value : "Disabled";
            settingDefaultMount_Select.ValueChanged += delegate {
                Module._settingDefaultMountChoice.Value = settingDefaultMount_Select.SelectedItem;
            };
            Label settingDefaultWaterMount_Label = new Label()
            {
                Location = new Point(0, settingDefaultMount_Select.Bottom + 6),
                Width = labelWidth2,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = mountsLeftPanel,
                Text = "Default water mount: ",
            };
            Dropdown settingDefaultWaterMount_Select = new Dropdown()
            {
                Location = new Point(settingDefaultWaterMount_Label.Right + 5, settingDefaultWaterMount_Label.Top - 4),
                Width    = mountsAndRadialInputWidth,
                Parent   = mountsLeftPanel,
            };
            settingDefaultWaterMount_Select.Items.Add("Disabled");
            var mountNamesWater = Module._mounts.Where(m => m.IsWaterMount).Select(m => m.Name);
            foreach (string i in mountNamesWater)
            {
                settingDefaultWaterMount_Select.Items.Add(i.ToString());
            }
            settingDefaultWaterMount_Select.SelectedItem = mountNamesWater.Any(m => m == Module._settingDefaultWaterMountChoice.Value) ? Module._settingDefaultWaterMountChoice.Value : "Disabled";
            settingDefaultWaterMount_Select.ValueChanged += delegate {
                Module._settingDefaultWaterMountChoice.Value = settingDefaultWaterMount_Select.SelectedItem;
            };
            Label settingDefaultMountKeybind_Label = new Label()
            {
                Location = new Point(0, settingDefaultWaterMount_Select.Bottom + 6),
                Width = labelWidth2,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = mountsLeftPanel,
                Text = "Keybind: ",
            };
            KeybindingAssigner settingDefaultMount_Keybind = new KeybindingAssigner(Module._settingDefaultMountBinding.Value)
            {
                NameWidth  = 0,
                Size       = new Point(mountsAndRadialInputWidth, 20),
                Parent     = mountsLeftPanel,
                Location   = new Point(settingDefaultMountKeybind_Label.Right + 4, settingDefaultMountKeybind_Label.Top - 1),
            };
            Label settingDisplayMountQueueing_Label = new Label()
            {
                Location = new Point(0, settingDefaultMountKeybind_Label.Bottom + 6),
                Width = labelWidth2,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = mountsLeftPanel,
                Text = "Display out of combat queueing:"
            };
            Checkbox settingDisplayMountQueueing_Checkbox = new Checkbox()
            {
                Size = new Point(labelWidth2, 20),
                Parent = mountsLeftPanel,
                Checked = Module._settingDisplayMountQueueing.Value,
                Location = new Point(settingDisplayMountQueueing_Label.Right + 5, settingDisplayMountQueueing_Label.Top - 1),
            };
            settingDisplayMountQueueing_Checkbox.CheckedChanged += delegate {
                Module._settingDisplayMountQueueing.Value = settingDisplayMountQueueing_Checkbox.Checked;
            };


            Label settingMountRadialSettingsMount_Label = new Label()
            {
                Location = new Point(0, settingDisplayMountQueueing_Label.Bottom + 24),
                Width = labelWidth2,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = mountsLeftPanel,
                Text = "Radial settings: ",
            };
            Label settingMountRadialSpawnAtMouse_Label = new Label()
            {
                Location = new Point(0, settingMountRadialSettingsMount_Label.Bottom + 6),
                Width = labelWidth2,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = mountsLeftPanel,
                Text = "Spawn at mouse: ",
            };
            Checkbox settingMountRadialSpawnAtMouse_Checkbox = new Checkbox()
            {
                Size = new Point(labelWidth2, 20),
                Parent = mountsLeftPanel,
                Checked = Module._settingMountRadialSpawnAtMouse.Value,
                Location = new Point(settingMountRadialSpawnAtMouse_Label.Right + 5, settingMountRadialSpawnAtMouse_Label.Top - 1),
            };
            settingMountRadialSpawnAtMouse_Checkbox.CheckedChanged += delegate {
                Module._settingMountRadialSpawnAtMouse.Value = settingMountRadialSpawnAtMouse_Checkbox.Checked;
            };
            Label settingMountRadialRadiusModifier_Label = new Label()
            {
                Location = new Point(0, settingMountRadialSpawnAtMouse_Label.Bottom + 6),
                Width = labelWidth2,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = mountsLeftPanel,
                Text = "Radius: ",
            };
            TrackBar settingMountRadialRadiusModifier_Slider = new TrackBar()
            {
                Location = new Point(settingMountRadialRadiusModifier_Label.Right + 5, settingMountRadialRadiusModifier_Label.Top),
                Width    = mountsAndRadialInputWidth,
                MaxValue = 100,
                MinValue = 20,
                Value    = Module._settingMountRadialRadiusModifier.Value * 100,
                Parent   = mountsLeftPanel,
            };
            settingMountRadialRadiusModifier_Slider.ValueChanged += delegate { Module._settingMountRadialRadiusModifier.Value = settingMountRadialRadiusModifier_Slider.Value / 100; };
            Label settingMountRadialIconSizeModifier_Label = new Label()
            {
                Location = new Point(0, settingMountRadialRadiusModifier_Label.Bottom + 6),
                Width = labelWidth2,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = mountsLeftPanel,
                Text = "Icon size: ",
            };
            TrackBar settingMountRadialIconSizeModifier_Slider = new TrackBar()
            {
                Location = new Point(settingMountRadialIconSizeModifier_Label.Right + 5, settingMountRadialIconSizeModifier_Label.Top),
                Width    = mountsAndRadialInputWidth,
                MaxValue = 100,
                MinValue = 5,
                Value    = Module._settingMountRadialIconSizeModifier.Value * 100,
                Parent   = mountsLeftPanel,
            };
            settingMountRadialIconSizeModifier_Slider.ValueChanged += delegate { Module._settingMountRadialIconSizeModifier.Value = settingMountRadialIconSizeModifier_Slider.Value / 100; };
            Label settingMountRadialIconOpacity_Label = new Label()
            {
                Location = new Point(0, settingMountRadialIconSizeModifier_Label.Bottom + 6),
                Width = labelWidth2,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = mountsLeftPanel,
                Text = "Icon opacity: ",
            };
            TrackBar settingMountRadialIconOpacity_Slider = new TrackBar()
            {
                Location = new Point(settingMountRadialIconOpacity_Label.Right + 5, settingMountRadialIconOpacity_Label.Top),
                Width    = mountsAndRadialInputWidth,
                MaxValue = 100,
                MinValue = 5,
                Value    = Module._settingMountRadialIconOpacity.Value * 100,
                Parent   = mountsLeftPanel,
            };
            settingMountRadialIconOpacity_Slider.ValueChanged += delegate { Module._settingMountRadialIconOpacity.Value = settingMountRadialIconOpacity_Slider.Value / 100; };
            Label settingMountRadialCenterMountBehavior_Label = new Label()
            {
                Location = new Point(0, settingMountRadialIconOpacity_Label.Bottom + 6),
                Width = labelWidth2,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = mountsLeftPanel,
                Text = "Center mount: ",
            };
            Dropdown settingMountRadialCenterMountBehavior_Select = new Dropdown()
            {
                Location = new Point(settingMountRadialCenterMountBehavior_Label.Right + 5, settingMountRadialCenterMountBehavior_Label.Top - 4),
                Width    = mountsAndRadialInputWidth,
                Parent   = mountsLeftPanel,
            };
            foreach (string i in Module._mountRadialCenterMountBehavior)
            {
                settingMountRadialCenterMountBehavior_Select.Items.Add(i.ToString());
            }
            settingMountRadialCenterMountBehavior_Select.SelectedItem = Module._settingMountRadialCenterMountBehavior.Value;
            settingMountRadialCenterMountBehavior_Select.ValueChanged += delegate {
                Module._settingMountRadialCenterMountBehavior.Value = settingMountRadialCenterMountBehavior_Select.SelectedItem;
            };
            Label settingMountRadialRemoveCenterMount_Label = new Label()
            {
                Location = new Point(0, settingMountRadialCenterMountBehavior_Label.Bottom + 6),
                Width = labelWidth2,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = mountsLeftPanel,
                Text = "Remove center mount from radial: ",
            };
            Checkbox settingMountRadialRemoveCenterMount_Checkbox = new Checkbox()
            {
                Size = new Point(labelWidth2, 20),
                Parent = mountsLeftPanel,
                Checked = Module._settingMountRadialRemoveCenterMount.Value,
                Location = new Point(settingMountRadialRemoveCenterMount_Label.Right + 5, settingMountRadialRemoveCenterMount_Label.Top - 1),
            };
            settingMountRadialRemoveCenterMount_Checkbox.CheckedChanged += delegate {
                Module._settingMountRadialRemoveCenterMount.Value = settingMountRadialRemoveCenterMount_Checkbox.Checked;
            };
            Label settingMountRadialToggleActionCameraKeyBinding_Label = new Label()
            {
                Location = new Point(0, settingMountRadialRemoveCenterMount_Label.Bottom + 6),
                Width = labelWidth2,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = mountsLeftPanel,
                Text = "Action camera keybind: ",
            };
            KeybindingAssigner settingMountRadialToggleActionCameraKeyBinding_Keybind = new KeybindingAssigner(Module._settingMountRadialToggleActionCameraKeyBinding.Value)
            {
                NameWidth = 0,
                Size = new Point(mountsAndRadialInputWidth, 20),
                Parent = mountsLeftPanel,
                Location = new Point(settingMountRadialToggleActionCameraKeyBinding_Label.Right + 4, settingMountRadialToggleActionCameraKeyBinding_Label.Top - 1),
            };

            #endregion

            #region OtherPanel
            Label settingDisplay_Label = new Label() {
                Location = new Point(0, 4),
                Width = labelWidth,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = otherPanel,
                Text = "Display: ",
            };
            Dropdown settingDisplay_Select = new Dropdown() {
                Location = new Point(settingDisplay_Label.Right + 5, settingDisplay_Label.Top - 4),
                Width = 160,
                Parent = otherPanel,
            };
            foreach (string s in Module._mountDisplay) {
                settingDisplay_Select.Items.Add(s);
            }
            settingDisplay_Select.SelectedItem = Module._settingDisplay.Value;
            settingDisplay_Select.ValueChanged += delegate {
                Module._settingDisplay.Value = settingDisplay_Select.SelectedItem;
            };
            Label settingDisplayCornerIcons_Label = new Label()
            {
                Location = new Point(0, settingDisplay_Label.Bottom + 6),
                Width = bindingWidth,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = otherPanel,
                Text = "Display Corner Icons: ",
            };
            Checkbox settingDisplayCornerIcons_Checkbox = new Checkbox()
            {
                Size = new Point(bindingWidth, 20),
                Parent = otherPanel,
                Checked = Module._settingDisplayCornerIcons.Value,
                Location = new Point(settingDisplayCornerIcons_Label.Right + 5, settingDisplayCornerIcons_Label.Top - 1),
            };
            settingDisplayCornerIcons_Checkbox.CheckedChanged += delegate {
                Module._settingDisplayCornerIcons.Value = settingDisplayCornerIcons_Checkbox.Checked;
            };
            Label settingDisplayManualIcons_Label = new Label()
            {
                Location = new Point(0, settingDisplayCornerIcons_Label.Bottom + 6),
                Width = bindingWidth,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = otherPanel,
                Text = "Display Manual Icons: ",
            };
            Checkbox settingDisplayManualIcons_Checkbox = new Checkbox()
            {
                Size = new Point(bindingWidth, 20),
                Parent = otherPanel,
                Checked = Module._settingDisplayManualIcons.Value,
                Location = new Point(settingDisplayManualIcons_Label.Right + 5, settingDisplayManualIcons_Label.Top - 1),
            };
            settingDisplayManualIcons_Checkbox.CheckedChanged += delegate
            {
                Module._settingDisplayManualIcons.Value = settingDisplayManualIcons_Checkbox.Checked;
                DisplayManualPanelIfNeeded(manualPanel);
            };
            #endregion

            #region manual Panel
            Label settingManual_Label = new Label() {
                Location = new Point(0, 2),
                Width = manualPanel.Width,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = manualPanel,
                Text = "Manual Settings",
                HorizontalAlignment = HorizontalAlignment.Center
            };

            Label settingManualOrientation_Label = new Label() {
                Location = new Point(0, settingManual_Label.Bottom + 6),
                Width = 75,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = manualPanel,
                Text = "Orientation: ",
            };
            Dropdown settingManualOrientation_Select = new Dropdown() {
                Location = new Point(settingManualOrientation_Label.Right + 5, settingManualOrientation_Label.Top - 4),
                Width = 100,
                Parent = manualPanel,
            };
            foreach (string s in Module._mountOrientation) {
                settingManualOrientation_Select.Items.Add(s);
            }
            settingManualOrientation_Select.SelectedItem = Module._settingOrientation.Value;
            settingManualOrientation_Select.ValueChanged += delegate {
                Module._settingOrientation.Value = settingManualOrientation_Select.SelectedItem;
            };

            Label settingManualWidth_Label = new Label() {
                Location = new Point(0, settingManualOrientation_Label.Bottom + 6),
                Width = 75,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = manualPanel,
                Text = "Icon Width: ",
            };
            TrackBar settingImgWidth_Slider = new TrackBar() {
                Location = new Point(settingManualWidth_Label.Right + 5, settingManualWidth_Label.Top),
                Width = 220,
                MaxValue = 200,
                MinValue = 0,
                Value = Module._settingImgWidth.Value,
                Parent = manualPanel,
            };
            settingImgWidth_Slider.ValueChanged += delegate { Module._settingImgWidth.Value = (int)settingImgWidth_Slider.Value; };

            Label settingManualOpacity_Label = new Label() {
                Location = new Point(0, settingManualWidth_Label.Bottom + 6),
                Width = 75,
                AutoSizeHeight = false,
                WrapText = false,
                Parent = manualPanel,
                Text = "Opacity: ",
            };
            TrackBar settingOpacity_Slider = new TrackBar() {
                Location = new Point(settingManualOpacity_Label.Right + 5, settingManualOpacity_Label.Top),
                Width = 220,
                MaxValue = 100,
                MinValue = 0,
                Value = Module._settingOpacity.Value * 100,
                Parent = manualPanel,
            };
            settingOpacity_Slider.ValueChanged += delegate { Module._settingOpacity.Value = settingOpacity_Slider.Value / 100; };

            IView settingClockDrag_View = SettingView.FromType(Module._settingDrag, buildPanel.Width);
            ViewContainer settingClockDrag_Container = new ViewContainer() {
                WidthSizingMode = SizingMode.Fill,
                Location = new Point(0, settingManualOpacity_Label.Bottom + 3),
                Parent = manualPanel
            };
            settingClockDrag_Container.Show(settingClockDrag_View);
            #endregion
        }

        private static void DisplayManualPanelIfNeeded(Panel manualPanel)
        {
            if (Module._settingDisplayManualIcons.Value)
                manualPanel.Show();
            else
                manualPanel.Hide();
        }
    }
}
