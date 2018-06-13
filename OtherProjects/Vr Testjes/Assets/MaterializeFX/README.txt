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

1.0 - released

1.1 - added two albedo shader
	  added scripts for easy use without animator
	  the two shaders are combined into one

1.1.1 - made editor improvements (inaccessible settings are hidden)        
        added the ability to replace the original material for materialization and return the original back, after the completion of materialization
        added events of the beginning and the end of materialization / disintegration        
        added example of how to disintegrate when shot
        
1.1.2 - added standart shader (metallic setup)
        added ability to replace materials on object (runtime/editor modes)
		optimized shaders

1.1.3 - ??
		
you can check the progress of the update here: https://trello.com/b/znDrmIao
*this board can be deleted later

---------------------------------------------------------------------------------------------------------

Why there are two folders: materializefx and materializationfx:
the shaders and scripts have been reworked to add support for the standard shader and abilty to replace materials
now materialization fx folder is Obsolete, in the next updates it will be removed

ASE shaders are only needed for visualization, they are not supported by scripts

---------------------------------------------------------------------------------------------------------

Feel free to contact me with any questions and comments

pelengami@gmail.com

Best Regards,
Max