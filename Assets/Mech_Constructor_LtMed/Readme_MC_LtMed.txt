Thank you for the purchase! Hope the assets work well.
If they don't, please contact me: slsovest@gmail.com



______________________
Assembling the robots:


Legs, shoulders and cockpits contain sockets for mounting other parts, their names start with "Mount_".
Just find them in the hierarchy, drop the part in the corresponding container, and It'll snap into place.

- Start with legs. 
- The first container is in Legs->HIPS->Pelvis->Top->Mount_top. 
- Put the shoulders or the cockpit into "Mount_top".
- Find other containers inside shoulders and cockpit.

After the assembly, robots consist of many separate parts and, even with batching, produce high number of draw calls.
You may want to combine non-animated parts into a single mesh for the sake of optimization.

All weapons contain locators at their barrel ends (named "Barrel_end"). Rocket launchers contain multiple locators, for all of the rockets.

If you need a cap for the hole at the top of the legs, you can find it in the Models->Legs_top_cap.fbx. Just drop it in Legs->HIPS, then move from HIPS to Pelvis.


Assembling the Jets:

The vehicles are less modular than the mechs, but the process is the same.
You'll just need to find the "Mount_" objects for the cockpits and weapons inside them.



___________
Animations:


Idle and Jump_jetpack animation files contain several animations:
Idle: "Idle_simple" - frames 170-260
Jump_jetpack: "Jump_jet_start"(6-14), "Jump_jet_idle" - 15-55", "Jump_jet_land" - 56-67

Unlike the other weapons, the minigun hasn't the same animation for all levels (due to different barrel rotation speed), check its Animator Controller.
Jetpack Jump
Switch to "jump_idle" after the "jump_start". You may want to tilt the the mech forward or backward when mech is flying. Switch to "jump_land" when it's time to land.
Simple jump
Consists of only one animation, not as flexible as the Jetpack jump. Hope it still could be useful in some projects.

If you want mech weapons to bounce a bit while running or walking,
you may drop the "Top_anim_weapons_bounce" prefab into the top part after mech is assembled, and drag the weapons into "Top_anim_weapons_bounce/Mount_weapons" container.

If you want to tweak the animations or create the new ones, the source .ma file contains the animated parts with their rigs.



________________
The texture PSD:


PBR:
To create the material in Unity 5, just plug in the textures from the "Materials/PBR_Maps/Unity_5_Standard_Shader (Specular)"
into corresponding inputs of Unity 5 standard shader (Specular workflow).

Decided not to include the PBR source PSD's directly in the package - they weigh a lot and not sure if they're needed by many people.
Here's the link:
https://drive.google.com/open?id=0B2mY9IjHMQLbNWlHYThFdk9VX1U
The .rar contains DDo 2.0 project, which consists of several PSD files that you can edit manually as well.
To create new mech colors, edit the Albedo one (mostly the layers under Body_Paint group).


Hand-painted:
The hand painted source .PSD can be downloaded here:
https://drive.google.com/file/d/1vrKOsOlL3ZY2xgGaN8akGprVmr-yWUSF/view?usp=sharing

For a quick repaint, adjust the layers in the "COLOR" folder. You can drop your decals and textures (camouflage, for example) in the folder as well. Just be careful with texture seams.
You may want to turn off the "FX_Rust" and "FX_Chipped_paint" layers for more cartoony look.
Or make ambient occlusion stronger by increasing opacity of "SHADING/MORE_OCCLUSION" layer.


________
Updates:


Version 1.1

Added skinned legs, cannons and machineguns. The models consist only of a single mesh object, and require 1 draw call (instead of 12 for the old legs, my bad). But do not batch.
Replaced the old prefabs with the new ones. The old ones can be found in Prefabs/Non skinned folder.
They may still work better (because of batching), if you have a lot of robots in a scene.


Version 1.2

Replaced in-place legs animations with animations with root motion. Hope they will work fine with Mecanim. If not, please, write. 
The old animations are in Animations/Legs_In_place_animations.rar.
The legs pivot was slightly off center, fixed. (Thanks to Devon G. for pointing to that.)


Version 1.3

4 new parts added:
- Shoulders_lt_frame_upgrade
- Shoulders_med_frame_upgrade
- Shoulders_med_shield_upgrade
- Cockpit_jet_upgrade

New animations added:
- Jetpack jump
- Simple jump
- Fall


Version 1.4 (January 2016)

Root motion finally fixed (had to add an additional parent bone to all the legs and animations).
Walk and run animation tweaked a bit.
Turn on place animation added.
Added HalfShoulder parts (can be very useful with the Spiders and Tanks pack).


Version 1.5 (May 2016)

New animations added:
- Turning while walking
- Turning while running
- Faster turning on place


Version 2.0 (July 2016):

- Normal map and PBR textures added.
- Added simple modular jets (5 levels)


Version 2.1 (September 2017):

- Refined normal map and all the PBR textures


Version 2.2 (August 2023):

- Equalized the texture brightness across all the Mech Constructor packages
- Added Metalness map
- Added HDRP Mask map