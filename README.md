# AtmosAudioFix
If you've played the VR build, you might've heard strange sounds while playing that you've never heard before. That's because these sounds are currently bugged. This ""fixes"" these sounds to make them hearable in normal gameplay.    

## Here is what you should be able to hear:    

Rank 0-1: Weird blblblblbl noises (Called "mag" sounds in the files). And wind. And cyberpunk-ish swells. Undescribable.  
Rank 2: Truck noises that you'd hear outside your bedroom window at 3am. And owls.  
Rank 3: Rain!!!!!!  
Rank 4: Low rumble of fire. And the fire in question exploding. Pretty loud.  
Rank 5: Birds.

## Installation  
(you need BepInEx for this)  
Extract the contents of the zip file to your plugins folder.    

**Recommended: BepInEx Config Manger, to increase/decrease the volume on the fly.**

## Why were the sounds bugged?
Receiver 2 uses FMOD for its audio. In older versions of the game, atmos audio were played by emitters that were placed roughly north and south.  
These emitters still exist, but they only play when you get REALLY close to them (often requiring you to noclip a floor down into the tile).  
My guess is that the current FMOD version changed the way emitters work, and made them only play when you get really close.  
I haven't found a way (that works) to increase the hearable range of the emitters, so I just parented one of them on the player, lol.
