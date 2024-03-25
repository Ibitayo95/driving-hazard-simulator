# VR DRIVING SIMULATOR

Virtual reality (VR) technology has demonstrated that it may be a very valuable and successful training tool for safety simulations. Because of the risk that learner and inexperienced drivers pose to themselves and other road users, it was proposed to create a VR simulation focusing on hazard perception. This project aims to enhance the present 3D testing and training methodologies used for the UK theory driving exam. Unity was used to build a virtual driving environment in which users may practise spotting road dangers and receive feedback on their performance. This is dynamic in nature, with a variety of randomised dangers, day/night cycles, and unpredictable weather patterns. The simulator renders 3D driving scenes using the Unity game engine and incorporates Unity's XR interaction framework for playback across multiple platforms. A variety of evaluations (technology acceptance model, cognitive walkthrough, technical evaluation) revealed that the optimised high-fidelity simulation was simple to use, realistic, immersive, and performant. 
This study provides a solid foundation for using immersive VR technology to improve hazard awareness training, paving the way for future advancements that may eventually improve road safety. 

## Navigating the code
All code is located within driving-hazard-simulator/Assets/Scripts/
### Folders
- General:  Utility classes for specialised use cases e.g. Simulation config, Day/Night Cycle script, Menu Scene tasks (Start simulation, exit simulation)
- Hazard Management: Consists of main logic and scripting for hazards, hazard detection, analysis and summary.
- InterfacesAndTypes: Foundational unchanging entities e.g. HazardType
- Pathfinding: A* search pathfinding algorithm for random pedestrian movement - credit to Low Poly Epic City by PolyPerfect
- PedestrianBehaviour: Scripting for hazard humans and normal pedestrians
- Traffic: Scripting for traffic cars and traffic light intersections
- VehicleBehaviour: Scripting for hazard cars and the user car (which follows a waypoint system and drives automatically using the wheelcolliders and the unity physics engine)
## The environment
![TitleScreen2](https://github.com/Ibitayo95/driving-hazard-simulator/assets/71972724/dda41aaa-e09f-4882-b9f2-c4b841235ba3)
![StreetViewArialDay](https://github.com/Ibitayo95/driving-hazard-simulator/assets/71972724/9dfd8606-4361-405d-bc8e-7d8eb5fb3e8c)
![FidelityComparisonHigh](https://github.com/Ibitayo95/driving-hazard-simulator/assets/71972724/649f3189-eab5-43fd-ac6d-d4bea5d2dcf9)
![StreetViewRain3](https://github.com/Ibitayo95/driving-hazard-simulator/assets/71972724/05a4a6f2-5205-41d8-8a5d-f3a95740dba6)
![SummaryScreen1](https://github.com/Ibitayo95/driving-hazard-simulator/assets/71972724/896fdb32-eb8e-4783-bafd-6bd351b33d65)

## Resources and packages used
- Unity 2022 LTS
- AllSky Skybox Set: RPGWhitelock
-	PBR Grass Textures: VIS Games
-	Stylized Vehicles Pack (Low Poly): Alex Lenk
-	Low Poly Epic City: PolyPerfect
-	COZY: Stylized Weather 2: Distant Lands
-	100+ PBR Materials Pack: Integrity Software & Games
-	3D rigged human characters: Mixamo
-	Human animations: Mixamo
