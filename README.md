# LittleDenham-Code-
This is the SMAPI component for the upcoming Little Denham mod.

It can be broken down into three major functions:

=== 1 - Lifts (Lifts.cs) ===

A very simple kitbash of some of the code used for Ridgeside Village's minecarts (modified with permission) in order to add a functional lift/elevator mechanic to the block of flats in Little Denham. From a player's point of view this does not become accessible until quite late in the story, but it functionally warps the player to the selected floor and plays the "crystal" noise.

=== 2 - Marnie's Animals (Animals.cs) ===

A rehash of the vanilla (and SVE) code for loading in Marnie's backyard animals on the Forest map. It effectively clears the list of spawned animals and generates a new set to ensure that there will not be cows or goats crossing the player's path on the route between Little Denham and the Forest. The only possible issue that may arise from this is that any other mods which load in "fake" farmyard animals may also see those lists wiped. Please contact me if this is the case for a mod you've created and I'll adapt it if possible.

=== 3 - Delayed Door Access (EnterFlat.cs) ===

This replicates the functionality of the Sewer grate when first obtaining the Rusty Key. You get a popup message the first time, sends the player a letter (which is also used as a flag in the code), and then the warp functions as normal. Within gameplay I've locked this functionality behind an event, but it's so early on in the story of the mod that most people won't even notice.
