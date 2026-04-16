# Achromatic DEVLOG


## Wed 2026-01-28 – Cheap Version Prototype

### Erik’s Prototype

![Image1](./DEVLOG.assets/DEVLOG.2026-01-28.image1.png)

![Image2](./DEVLOG.assets/DEVLOG.2026-01-28.image2.png)

For this week's cheap prototype, I have produced a continuation of previous ideas, yet a completely different fundamental implementation (from the code perspective) in an attempt to achieve similar ideas / player experience. It turns out the previous approach of using physics, timing, and speed control to sync music was unsuccessful and especially unreliable (the sync would break during different builds, when switching to another device, etc.). I have reimplemented the player's control method to support script-based and event-driven control of the player character, using the C# event/delegate system to minimize input lag. There was not a huge detectable lag when using Unity's message feature, but my research has informed me to use C# events. Additionally, I have defined a JavaScript Object Notation (JSON)-based customized beatmap file format, as there is no universal beatmap format (at least none that is open-source and well documented). In support of this, I have also written a basic parser for such a format that places blocks/indication of the note on the map dynamically during runtime.

#### buildnote

this prototype implements a minimal, playable slice of the final game: a single 2D platforming level plus a linked music/rhythm mode with basic transitions, placeholder art, and simplified physics. it demonstrates core gameplay loops and mode-switching without full content or polish

explore mode: WSAD for movement, Space for interactions
music mode: WSD for movement, Space to trigger notes

background music — "City Lights" by tubebackr & HiLau (distributed via Audio Library on YouTube); license: Creative Commons Attribution-NoDerivs 3.0 Unported (CC BY-ND 3.0); requires attribution, prohibits derivative works and removal/alteration of credits, and may require direct permission for uses outside YouTube

## Ying’s Prototype

Based on the playtest from first prototypes, most of the time players can’t recognize original hit or miss feedback, which did not feel any satisfaction when playing. So this week I’ve changed the effect of hit plus the text feedback (perfect/cool/miss) to let the player recognize easier when they hit.
In addition, I’ve created my own music beatmap editor for my prototypes to edit and place each note easier compared to before that I had to edit on the json file directly. Also coordinated current scripts on the song manager for songs selection in the future.
So the current cheap prototype is a music game where the white dot is the player and needs to go up and down, and use the spacebar to hit the notes. Pink notes are single press, blue notes are long press, purple notes are still working on, which originally they are the tap notes that requires player to tap more times.
This time, I still haven’t used any 3rd party libraries besides the Unity engine, and again, Gemini helps me a lot with coding. The temporary song I chose is Hikari from Punishing Gray Raven, and it does have copyright.

![Image3](./DEVLOG.assets/DEVLOG.2026-01-28.image3.png)

![Image4](./DEVLOG.assets/DEVLOG.2026-01-28.image4.png)




































## Wed 2026-01-21 - First Prototypes

### Erik’s Prototype

I have set up various systems, including the input and audio systems. I created a 2D platformer using placeholder assets to explore merging platformers with rhythm-based games. The core gameplay is: jumping on up arrows, and dashing on forward arrows. All these actions are synced to the music. The player moves along a simple platform, with arrows indicating jump timing (matching on the beat.) The current map is composed majorly with tilemap, but I have found using tilemap + rigidbody physics is difficult & time consuming during the development process. Considering dynamically scrolling the camera & placing the obstacles in future builds.

![Image3](./DEVLOG.assets/DEVLOG.2026-01-21.image3.png)

![Image4](./DEVLOG.assets/DEVLOG.2026-01-21.image4.png)

### Erik’s build notes

- question: is there a way to combine a music game and a 2D platformer organically to bring satisfaction to the user
- hypothesis: the Music Component will provide short-term satisfaction through tight audio sync and clear, immediate feedback. The Platformer Part, augmented with story and progression, will provide longer-term engagement and satisfaction
- inputs: currently supports two controls — Space to jump and D to dash.
- third-party assets and licenses:

  - platform game assets — Bayat Games, Unity Asset Store; license: Standard Unity Asset Store EULA (Extension Asset); copyright held by Bayat Games

  - background music — "City Lights" by tubebackr & HiLau (distributed via Audio Library on YouTube); license: Creative Commons Attribution-NoDerivs 3.0 Unported (CC BY-ND 3.0); requires attribution, prohibits derivative works and removal/alteration of credits, and may require direct permission for uses outside YouTube

### Ying’s Prototype

![Image1](./DEVLOG.assets/DEVLOG.2026-01-21.image1.png)

![Image2](./DEVLOG.assets/DEVLOG.2026-01-21.image2.jpg)

The hypothesis that I’m investigating is letting the player gain satisfaction from a music rhythm-based game, but also cooperate with simple lines and shapes through my prototype.  And I use my prototype to answer the question of “Is there a new mechanic of music rhythm-based game that I haven’t tried out?”

I use simple shapes, trails, and particle effects to make the music-based rhythm game with the main mechanic of the judgement line being stable, but since the player keeps “moving”, the player approaches the music notes that they have to hit. To operate the player object, the player simply needs to do up and down by using w and s from the keyboard and do hit by using the space key. Super basic visuals for now and it’s all about getting the ball physics and paddle control feeling smooth.

I find an interesting point when I’m working on the ideation and this prototype, that most of the music games in the market, especially popular ones, tend to be simple in input and traditional in mechanics. To find a balance between new and traditional mechanics, this actually takes a longer time to brainstorm.

The hypothesis that I’m investigating is letting the player gain satisfaction from a music rhythm-based game, but also cooperate with simple lines and shapes through my prototype.  And I use my prototype to answer the question of “Is there a new mechanic of music rhythm-based game that I haven’t tried out?”

I use simple shapes, trails, and particle effects to make the music-based rhythm game with the main mechanic of the judgement line being stable, but since the player keeps “moving”, the player approaches the music notes that they have to hit. To operate the player object, the player simply needs to do up and down by using w and s from the keyboard and do hit by using the space key. Super basic visuals for now and it’s all about getting the ball physics and paddle control feeling smooth.

I find an interesting point when I’m working on the ideation and this prototype, that most of the music games in the market, especially popular ones, tend to be simple in input and traditional in mechanics. To find a balance between new and traditional mechanics, this actually takes a longer time to brainstorm.

The majority of the assets all came from Unity URP by default. I didn’t import any other libraries, but most of the codes are helped by Gemini and Grok AI.
