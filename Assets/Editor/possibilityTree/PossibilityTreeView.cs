using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class PossibilityTreeView : EditorWindow
{
    [MenuItem("Window/UI Toolkit/PossibilityTreeView")]
    public static void ShowExample()
    {
        PossibilityTreeView wnd = GetWindow<PossibilityTreeView>();
        wnd.titleContent = new GUIContent("PossibilityTreeView");
    }

    public void CreateGUI()
    {   
        VisualElement rootVisualElement = new VisualElement();
        rootVisualElement.AddToClassList("main-panel");

        // root della visual element
        base.rootVisualElement.Add(rootVisualElement);
        // if "Null" allora il visual element non viene renderizzato in nessun visual ovvero non è collegato a nulla
        //Debug.Log(rootVisualElement.panel);


        // foglio di stile UI
        StyleSheet styleSheet = (StyleSheet) EditorGUIUtility.Load("PossibilityTreeView.uss");
        
        // inserire il foglio di stile all'interno della gerarchia usando un VisualElement
        if(styleSheet != null)
        {
            rootVisualElement.styleSheets.Add(styleSheet);// inietto nel visual element root lo stile
        }
        


        Label label = new Label("titolo");
        Label label2 = new Label("titolo2");

        // inserisci l'istanza in un'istanza visual element
        rootVisualElement.Add(label);

        /* a differenza di add, in insert si può stabilire l'indice(ordine specifico) in 
         * cui inserire l'istanza nell'istanza del visual element
         * è comunque rispettato il Drawing order 
        */
        rootVisualElement.Insert(0, label2);


        // creazione color picker
        Label title = new Label("Color Picker");
        // modifiche di stile statiche(usare fogli di stile)
        title.style.color = Color.white;
        rootVisualElement.Add(title);
        ColorField colorPicker = new ColorField();
        //selezione stile colorPicker
        colorPicker.AddToClassList("color-picker");
        rootVisualElement.Add(colorPicker);
        


        // crezione bottoni
        VisualElement buttonsContainer = new VisualElement();
        Button randomButton = (Button)createButton("Random color");
        Button resetColor = (Button) createButton("Reset color");
        Button copyButton = (Button) createButton("Copy color");
        Button pasteButton = (Button) createButton("Paste color");
        //iniezione classe container bottoni
        buttonsContainer.AddToClassList("horizontal-container");

        // iniezione classi stileSheet
        // aggiungi classe per utilizzare selettore di classe tramite stylesheet 
        randomButton.AddToClassList("dark-button");
        resetColor.AddToClassList("dark-button");
        copyButton.AddToClassList("dark-button");
        pasteButton.AddToClassList("dark-button");

        buttonsContainer.Add(randomButton);
        buttonsContainer.Add(resetColor);
        buttonsContainer.Add(copyButton);
        buttonsContainer.Add(pasteButton);

        rootVisualElement.Add(buttonsContainer);
    }

    private VisualElement createButton(string buttonString)
    {
        VisualElement button = new VisualElement();


        /*
         * utilizzo dell'object initializer 
         * https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/object-and-collection-initializers
         */
        return new Button() { text = buttonString }; 
    }
}