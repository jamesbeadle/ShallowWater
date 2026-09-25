# Playing the current build

1. Clone the repository and open the folder in Unity Hub with Unity 6 LTS (any 6000.0 release; accept the version prompt).
2. Open any scene, or make a new empty one. Press Play.
3. You are on *Sparrow*, moored at Hopwas, pointing south towards Fazeley. W or up is ahead, S or down is astern, A and D or the arrows are the rudder. The boat only answers the helm once it has steerage way.
4. The water ends at the planks at Huddlesford and the chain at Fazeley. It holds you there. That is the pound.

The game logic lives in `Assets/Game` and knows nothing about Unity. `Assets/Unity` connects it to the scene, and builds the whole pound at runtime so no scene file has to be kept in step with the code yet.
