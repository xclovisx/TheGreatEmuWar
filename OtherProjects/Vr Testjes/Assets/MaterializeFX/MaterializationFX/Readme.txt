Thanks for you purshase!

---------------------------------------------------------------------------------------------------------
Please read how to add bloom:
---------------------------------------------------------------------------------------------------------

HELPFUL NOTE:
For correct work as in demo scene you need enable "HDR" on main camera and. 

https://www.assetstore.unity3d.com/en/#!/content/83912 link on free unity physically correct bloom.
Use follow settings:
Threshold 2
Radius 7
Intencity 1
High quality true
Anti flicker true

In forward mode, HDR does not work with antialiasing. So you need disable antialiasing (edit->project settings->quality)
or use deffered rendering mode.

Add Post processing behaviour(script) into your camera and choose PostProcessingProfile.asset

---------------------------------------------------------------------------------------------------------

Change log:

1.0 - Released
1.1 - Add two albedo shader. Added scripts for easy use without animator. The two shaders are combined into one
1.1.2 - ?? Add support of multiple materials/Disintegration by event (for example from a shot)
1.1.3 - ?? Unity shader graph supports
1.1.4 - ?? Gpu Particles


I'm sorry for the loss of backward compatibility, but it was necessary to combine the shaders

---------------------------------------------------------------------------------------------------------

Feel free to contact me with any questions and comments

Since yours,
Max