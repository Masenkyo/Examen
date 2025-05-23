<p align="center">
  <img src="https://github.com/user-attachments/assets/8b6602dc-4fd0-44df-b895-cb9d44e220c5"></img>
</p>

Dit is een couch co-op spel dat makkelijk te begrijpen is en gespeeld kan worden door een breed scala aan mensen. Het doel is om samen te coordineren om een bal voorzichtig naar beneden te brengen door een soort flipper kast. De spelers besturen de flippers.

# [*Pipeline*](https://github.com/Masenkyo/Examen/wiki/Pipeline "Pipeline")

## [*Technical Design*](https://github.com/Masenkyo/Examen/wiki/Technical-design "Technical Design")
## [*Functional Design*](https://github.com/Masenkyo/Examen/wiki/Functional-Design "Functional Design")
  
### Sil zijn scripts:  
  
Flippers: [Code](https://github.com/Masenkyo/Examen/blob/master/Assets/Scripts/Flippers/Flipper.cs "Flippers"),  [Functional](https://github.com/Masenkyo/Examen/wiki/Functional-Design#flippers "Functional Design"), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#flippers "Technical Design")  
  
Moving Flipper: [Code](https://github.com/Masenkyo/Examen/blob/master/Assets/Scripts/Flippers/MovingFlipper.cs "Moving Flipper"), [Functional](https://github.com/Masenkyo/Examen/wiki/Functional-Design#flippers "Functional Design"), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#moving-flipper "Technical Design")  
  
Cutscene: [Code](https://github.com/Masenkyo/Examen/blob/master/Assets/Scripts/Camera/Cutscene.cs "Cutscene"),  [Functional](https://github.com/Masenkyo/Examen/wiki/Functional-Design#camera "Functional Design"), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#cutscene "Technical Design")  
  
LevelSystem: [Code](https://github.com/Masenkyo/Examen/blob/master/Assets/Scripts/Level%20Systeem/LevelSystem.cs "LevelSystem"),  [Functional](https://github.com/Masenkyo/Examen/wiki/Functional-Design#level-generation "Functional Design"), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#levelsystem "Technical Design")  
  
Ball: [Code](https://github.com/Masenkyo/Examen/blob/master/Assets/Scripts/Ball/Ball.cs "Ball"), [Functional](https://github.com/Masenkyo/Examen/wiki/Functional-Design#ball "Functional Design"), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#ball "Technical Design")  
  

### Boi zijn scripts:
  
Ball: [Code](https://github.com/Masenkyo/Examen/blob/master/Assets/Scripts/Ball/Ball.cs "Ball"), [Functional](), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#ball "Technical Design")  

Camera Follow: [Code](https://github.com/Masenkyo/Examen/blob/master/Assets/Scripts/Camera/Follow.cs "Camera Follow"), [Functional](), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#camera "Technical Design")  
  
Damage Phases: [Code](https://github.com/Masenkyo/Examen/blob/master/Assets/Scripts/Ball/Phases.cs "Damage Phases"), [Functional](), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#phases "Technical Design")  
  
Progress Bar: [Code](https://github.com/Masenkyo/Examen/blob/develop/Assets/Scripts/progressbar/ProgressBar.cs "Progress Bar"), [Functional](), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#progress-bar "Technical Design")  
  
Moving Obstacle: [Code](https://github.com/Masenkyo/Examen/blob/develop/Assets/Scripts/MovingObstacle/MovingObstacle.cs "Moving Obstacle"), [Functional](), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#moving-obstacle "Technical Design")  

### Teo zijn scripts:

Lobby Manager: [Code](https://github.com/Masenkyo/Examen/blob/develop/Assets/Scripts/Lobby/LobbyManager.cs "LobbyManager"), [Functional](https://github.com/Masenkyo/Examen/wiki/Functional-Design#lobby), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#lobby-&-gamemanager)  
  
Game Manager: [Code](https://github.com/Masenkyo/Examen/blob/develop/Assets/Scripts/GameManager/GameManager.cs "Game Manager"), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#lobby-&-gamemanager)  

Moving Flipper: [Code](https://github.com/Masenkyo/Examen/blob/develop/Assets/Scripts/Flippers/MovingFlipper.cs "Moving Flipper"), [Functional](https://github.com/Masenkyo/Examen/wiki/Functional-Design#flippers), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#flippers)  
  
Repair Prompt: [Code](https://github.com/Masenkyo/Examen/blob/develop/Assets/RepairPrompt.cs "Repair Prompt"), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#flippers)  
  
Game State Class: [Code](https://github.com/Masenkyo/Examen/blob/develop/Assets/Scripts/Lobby/GameStateClass.cs), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#lobby-&-gamemanager)  


## Flippers/Moving Flippers

Flipper: [Code](https://github.com/Masenkyo/Examen/blob/master/Assets/Scripts/Flippers/Flipper.cs "Flippers"),  [Functional](https://github.com/Masenkyo/Examen/wiki/Functional-Design#flippers "Functional Design"), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#flippers "Technical Design")  

Moving Flipper: [Code](https://github.com/Masenkyo/Examen/blob/master/Assets/Scripts/Flippers/MovingFlipper.cs "Moving Flipper"), [Functional](https://github.com/Masenkyo/Examen/wiki/Functional-Design#flippers "Functional Design"), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#moving-flipper "Technical Design")  

The players are controlling the flippers with their joystick to rotate and move them.  
Some flippers are controlled by 2 people at the same time which requires alot of teamwork to use.  
The flippers can sometimes break, so then your whole team needs to help you out to repair it by clicking on the X button continuously.  

https://github.com/user-attachments/assets/c50b9ee9-5e73-4ac8-9555-008d8e966144

## LevelSystem

LevelSystem: [Code](https://github.com/Masenkyo/Examen/blob/master/Assets/Scripts/Level%20Systeem/LevelSystem.cs "LevelSystem"),  [Functional](https://github.com/Masenkyo/Examen/wiki/Functional-Design#level-generation "Functional Design"), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#levelsystem "Technical Design")  

The LevelSystem takes all of the level prefabs and puts them in random orders underneath eachother with the specified settings.  
After everything has been placed, a end level will be placed at the bottom so you can complete the game.  

![446051058-0f2398af-213d-476c-b2d6-ee94ec4889cc](https://github.com/user-attachments/assets/d1103c39-1bc2-49be-9caf-c31b0ba639ed)

## Cutscene

Cutscene: [Code](https://github.com/Masenkyo/Examen/blob/master/Assets/Scripts/Camera/Cutscene.cs "Cutscene"),  [Functional](https://github.com/Masenkyo/Examen/wiki/Functional-Design#camera "Functional Design"), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#cutscene "Technical Design")  

It shows you the whole level when you start the game so you know what you can expect from the level.  

https://github.com/user-attachments/assets/42bc75c5-20d2-46ac-9c36-9cf6f76bb674 

## Camera

The camera follows the ball around smoothly for a good game experience.  

https://github.com/user-attachments/assets/b85fb452-f093-47fd-a561-15c51b0422f1

## Ball

Ball: [Code](https://github.com/Masenkyo/Examen/blob/master/Assets/Scripts/Ball/Ball.cs "Ball"), [Functional](https://github.com/Masenkyo/Examen/wiki/Functional-Design#ball "Functional Design"), [Technical](https://github.com/Masenkyo/Examen/wiki/Technical-design#ball "Technical Design")  

The ball physics are in this script and also manages at what velocities you take damage.  
When the ball dies it respawns as a new ball and you can also reset the ball by holding the triangle button.  

https://github.com/user-attachments/assets/2b80acd7-b7b1-4984-9aa7-2960a7470995

## Players/Lobby

You can join while in the lobby, but also while you're ingame, the flippers will get evenly distributed across all players even when they join/leave halfway through.  

![vlcsnap-2025-05-21-13h17m51s451](https://github.com/user-attachments/assets/579a8929-618a-4fc5-b59c-bc29b5a3e9f3)
filmpje van players die flippers hebben?

## PowerUps

You can get multiple effects from getting powerups, but make sure not to take the bad ones.  

https://github.com/user-attachments/assets/f52e66af-d3bd-45f4-96b6-4c932afeaf7e

## Checkpoint

If you touch the checkpoint, the next time the ball respawns it will spawn at that checkpoint so you don't have to do the whole level over, but it's not always easy to get a checkpoint.  

https://github.com/user-attachments/assets/7ae124ee-fa06-47b1-8832-d5d4404c9330

## Knives

The knives will destroy the ball if the ball touches the knives, the knives can also move towards any point you want them to go to.  

https://github.com/user-attachments/assets/9dc71798-3f38-446a-914d-e12f099403a7

## End Goal

The end goal is necessary to complete the game, you have to throw the product/ball into the bag.  

https://github.com/user-attachments/assets/3482e1d9-20e9-4b39-bbb0-bb29ea817c87

