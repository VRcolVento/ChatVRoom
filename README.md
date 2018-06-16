# Demo
Demo generico

PS
Giulio Auriemma mi aiuta :camel:

[Twity](https://github.com/toofusan/Twity)

[Github for Unity](https://github.com/github-for-unity/Unity)

[Unity XR](https://unity3d.com/learn/tutorials/s/xr)

- [ ] quando si fa load room, fare il loading della scena, altrimenti ora come ora gli oggetti si sommano a quelli già presenti

## Editor Mode

### In caso qualcuno perdesse i riferimenti agli scripts

* Prefabs
  - [X] DictionaryEntity.cs;
  - [X] Interactible.cs: Selected Material = Selected, Default Material = Defualt;
  - [ ] ModifyObject.cs: Feasible Mat = Toby, Unfeasible Mat = Red.
* User
  - [ ] UserPlaceObject.cs;
  - [X] UserChooseObject.cs: State Text = State Text (from editorScene).
  - Child: Camera:
    - [X] CameraBehaviour.cs.
* SceneController
  - [X] SceneController.cs.
* GUIController
  - [X] Creator.cs.
  - [X] GUIController.cs, attach menuCanvas script
* HelpCanvas
  - SaveButton: OnClick(), RunTimeOnly, **Ref**: GuiController, **Fun** Creator.SaveRoom
  - LoadButton: OnClick(), RunTimeOnly, **Ref**: GuiController, **Fun** Creator.LoadRoom
* MenuCanvas
  - [X] MenuScript.cs, attach defaultCamera.transform
    - [X] MenuButtonScript.cs;
    - Associa ai Trigger Component i metodi di menuButtonScript onEnter, onExit e press al Button Component
    
Piccoli appunti per me del futuro:
magari crea un prefab VRMenuButton con tutti i Component messi fatti stasera anziché fare gli stessi passi tutte le volte.
