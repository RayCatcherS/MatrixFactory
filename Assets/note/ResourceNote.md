# Special Folder Names - Editor and Resources // note
Folder with special names are reserved for special use. We can create the special folder anywhere we want and as many times as we want.
The script in the Editor folder that inherit MonoBehaviour class cannot be attached to the GameObjects.
Use the tag "EditorOnly" in the editor gameobject tag to remove the gameobject in the final build. This tag say that gameobject will not inclueded in the build

## Load method
Resources.Load method is used for runtime scripts, retrieve the resources by the all Resources folders (including Resources Editor folder)

The Resources in the Editor folder will be removed. Used to contain script editor files.
If the resources will be used in the final game build. Don't put them inside the Resources folder of the Editor folder, put the files in the Resources folder out the Editor folder

The file name is an unique id
To load resources from Resources folder use: "Resources.Load<TypeOfObject>("fileName");", PossibilityTreeView is the name of the file (case unsensitive), no file extension.
"TypeOfObject[] Resources.LoadAll<TypeOfObject>("fileName")", returns all files named fileName

Resources.Load is fine but it is better use it for runTime scripts

## Editor Load 
Unity provide a Load method for the Editor scripts. EditorGUIUtility.Load("fileName.ex") retrieve the resources only by the "Editor Default Resources" folder in the root of the project. In the fileName the extension in required

## other special folders
https://usunyu.com/note/2017/09/26/unity-special-folders/