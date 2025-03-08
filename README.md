# LittleDenham-Code-
This is the SMAPI component for the upcoming Little Denham mod.

It can be broken down into two functions:

=== 1 - Lifts (Lifts.cs) ===

A very simple kitbash of some of the code used for Ridgeside Village's minecarts (modified with permission) in order to add a functional lift/elevator mechanic to the block of flats in Little Denham. From a player's point of view this does not become accessible until quite late in the story, but it functionally warps the player to the selected floor and plays the "crystal" noise.

=== 2 - Delayed Door Access (EnterFlat.cs) ===

This replicates the functionality of the Sewer grate when first obtaining the Rusty Key. You get a popup message the first time, sends the player a letter (which is also used as a flag in the code), and then the warp functions as normal. Within gameplay I've locked this functionality behind an event, but it's so early on in the story of the mod that most people won't even notice.
