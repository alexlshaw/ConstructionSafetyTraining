# Construction Safety Training

### A training tool developed by the University of Auckland
*Lead programmer: Toby Tomkinson*

This is a construction training tool developed to educate construction workers working for City Rail Link in order to reduce the rate at which accidents occur (or prevent them occuring entirely).

This tool focuses on situational awareness around vehicles as well as safely and correctly navigating contruction sites. 


##Usage

The construction sites in this application are procedural and can be controlled by editing the file "config.cfg". Multiple variables are available to change. This includes the seed value that affects the consistency of site generation, the total number of sections within the site, their size and the target number of items to spawn within said site - this number is not the number of prefabs guarenteed to spawn as the sites are procedurally generated and will not spawn an item if a previously spawned item overlaps.

The currently available items available to spawn are the following:
* prefab1 (a concrete cube and placeholder item)
* Crane
* CraneVehicle
* Excavator
and can be added or removed by editing the "Prefabs" section of the config file. The chance of each item spawning is available in the "PrefabsChance" section and correspond to the same *ordered* prefab item - sequenced numbers are in place for seperation and have no effect on the corresponding prefab's sequenced number.

More documentation is available in the config file itself, though it's fairly self-explanatory.
