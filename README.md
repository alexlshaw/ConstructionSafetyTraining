# Construction Safety Training

### A training tool developed by the University of Auckland
> *Lead programmer: Toby Tomkinson*

This is a construction training tool developed to educate construction workers working for City Rail Link in order to reduce the rate at which accidents occur (or prevent them occuring entirely).

This tool focuses on situational awareness around vehicles as well as safely and correctly navigating contruction sites. 

### Current Functionality
Current functionality includes the following:
* Customisable site generation
* Customisable individual section generation
* External configuration file
* Simple VR implementation
* Animated crane with moving load
* Animated vehicles with risk zones
* Vehicle tracks
* AI roaming
* Basic waypoint objectives
* Basic models outside of site
* Ambient background noises and effects

### Future Functionality
Future functionality includes the following:
* Full-body VR tracking with individual finger tracking
* "Spotters" by vehicles
* Physics based interaction with AI
* Detailed models outside of site

## Usage

The construction sites in this application are procedural and can be controlled by editing the file "config.cfg". Multiple variables are available to change. This includes the seed value that affects the consistency of site generation, the total number of sections within the site, their size and the target number of items to spawn within said site - this number is not the number of prefabs guaranteed to spawn as the sites are procedurally generated and will not spawn an item if a previously spawned item overlaps.

The currently available items available to spawn are the following:
* prefab1 (a cube/placeholder item)
* BigWheel (an earth-mover truck)
* Building (a simple building shell)
* CraneVehicle (a truck-crane vehicle)
* Crane (a stationary crane with a concrete base)
* Crate (a shipping container)
* dirt (a pile of dirt)
* Excavator (it's in the name...)
* Mixer (a small cement mixer)
* Pallet (a pile of wooden pallets)

These can be added or removed by editing the prefabs section of the config file. The chance of each item spawning is the second option, e.g. "prefab={Pallet, 1}" is a pallet object with a 100% chance to spawn, while "prefab={CraneVehicle, 0.15}" is a crane vehicle with a 15% chance to spawn.

More documentation is available in the config file itself, though it's fairly self-explanatory.
