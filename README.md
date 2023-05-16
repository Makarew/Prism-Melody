# Prism Melody

A mod that enables custom color palettes in Idol Showdown

# Installation
### Melon Loader
Prism Melody uses Melon Loader as its mod loader. To install Melon Loader, first download the [latest release](https://github.com/LavaGang/MelonLoader/releases/latest/download/MelonLoader.x64.zip). Extract the contents of the .zip file into the root of your Idol Showdown installation, next to the .exe.

### Prism Melody
To install Prism Melody, download the [latest release](https://github.com/Makarew/Prism-Melody/releases/latest). Extract the "Mods" and "Plugins" folders from the .zip file into the root of your Idol Showdown installation, next to the .exe. 

### Color Palettes
Color palette files will be placed into "Idol Showdown/Plugins/Palettes". You need both "in" and "out" files for color palettes to work properly. To change the outfit the color palette is applied to, change the number in the file name to any number between 0 and 7. Prism Melody includes color palettes for Aki and Coco's 2nd outfits. 

# Create Color Palettes
### Getting The Tools
To create color palettes, you will first need [Unity 2020.1.6f1](https://unity.com/releases/editor/archive). It's recommend to install [Unity Hub](https://public-cdn.cloud.unity3d.com/hub/prod/UnityHubSetup.exe), and install the Unity editor through Unity Hub.

Once Unity is installed, you'll need the latest version of [Idol Showdown Studio](https://github.com/Makarew/Idol-Showdown-Studio/releases/latest). Extract the .zip file anywhere you like. Once done, open Unity Hub. Press "Open" in the top right and navigate to the folder you extracted from the Idol Showdown Studio .zip file. Once found, open the folder. Idol Showdown Studio should now appear in Unity Hub. Press it to open Idol Showdown Studio.

### Creating A Color Palette
In Idol Showdown Studio, you'll find a dropdown called "Idol Showdown". Press that, navigate to "Pallete Editor", and select a character. The Pallete Editor for that character will pop up. At the top of the editor, you'll have a dropdown to select the outfit you want to replace. On the left, you'll have a sprite for the character. This colors on the sprite will change as you make your edits. On the right, you have a list of colors. They all correspond to a color on the character's sprite. Click the color to bring a color wheel, or press the eye dropper to the right of the color to select any color currently on your screen.

If you want to start over, or the editor has an error, press "Reset Colors" at the bottom. This will bring the editor back to its default state. 

Once you're done creating your color palette, press "Export" at the bottom of the editor. This will create two .txt files in the "Export" folder. You can find this in the "Project" panel on the right of the main Unity window. The .txt files should have the format of character name, outfit number, and either in or out. Changing this format will cause Prism Melody to be unable to load the color palette. The only time you should edit the file names are if you want to change the outfit the color palette is applied to.

Both of these files are needed for the color palette to work properly. You'll place these in "Idol Showdown/Plugins/Palettes".
