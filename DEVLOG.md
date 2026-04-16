# Achromatic DEVLOG




## Wed 2026-01-21 – First Prototypes

### Erik’s Prototype

I have set up various systems, including the input and audio systems. I created a 2D platformer using placeholder assets to explore merging platformers with rhythm-based games. The core gameplay is: jumping on up arrows, and dashing on forward arrows. All these actions are synced to the music. The player moves along a simple platform, with arrows indicating jump timing (matching on the beat.) The current map is composed majorly with tilemap, but I have found using tilemap + rigidbody physics is difficult & time consuming during the development process. Considering dynamically scrolling the camera & placing the obstacles in future builds.

### Erik’s build notes

question: is there a way to combine a music game and a 2D platformer organically to bring satisfaction to the user
hypothesis: the Music Component will provide short-term satisfaction through tight audio sync and clear, immediate feedback. The Platformer Part, augmented with story and progression, will provide longer-term engagement and satisfaction
inputs: currently supports two controls — Space to jump and D to dash.
third-party assets and licenses:

platform game assets — Bayat Games, Unity Asset Store; license: Standard Unity Asset Store EULA (Extension Asset); copyright held by Bayat Games

background music — "City Lights" by tubebackr & HiLau (distributed via Audio Library on YouTube); license: Creative Commons Attribution-NoDerivs 3.0 Unported (CC BY-ND 3.0); requires attribution, prohibits derivative works and removal/alteration of credits, and may require direct permission for uses outside YouTube

### Ying’s Prototype

The hypothesis that I’m investigating is letting the player gain satisfaction from a music rhythm-based game, but also cooperate with simple lines and shapes through my prototype.  And I use my prototype to answer the question of “Is there a new mechanic of music rhythm-based game that I haven’t tried out?”

I use simple shapes, trails, and particle effects to make the music-based rhythm game with the main mechanic of the judgement line being stable, but since the player keeps “moving”, the player approaches the music notes that they have to hit. To operate the player object, the player simply needs to do up and down by using w and s from the keyboard and do hit by using the space key. Super basic visuals for now and it’s all about getting the ball physics and paddle control feeling smooth.

I find an interesting point when I’m working on the ideation and this prototype, that most of the music games in the market, especially popular ones, tend to be simple in input and traditional in mechanics. To find a balance between new and traditional mechanics, this actually takes a longer time to brainstorm.

The majority of the assets all came from Unity URP by default. I didn’t import any other libraries, but most of the codes are helped by Gemini and Grok AI.
