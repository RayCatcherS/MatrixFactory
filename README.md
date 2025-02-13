# MatrixFactory
Matrix Factory è un puzzle game sperimentale sviluppato in 4 mesi. Nel gioco si devono configurare i percorsi adeguati per consegnare la quantità sufficiente di pacchi per superare il livello. I pacchi bomba dovranno essere scartati con l'inceneritore affinchè non danneggino altri pacchi. In questo progetto ho sperimentato la generazione di livelli tramite un algoritmo di ricerca esaustiva. I livelli generati vengono poi valutati da un algoritmo che valuta uno scoring che ne rappresenta la difficoltà rispetto alle caratteristiche del livello generato. I livelli vengono presentati al giocatore in ordine di difficoltà crescente. Tutti gli assets, modelli 3D, effetti, illuminazione e UI sono stati realizzati da me.

L'obiettivo di questo progetto è stato quello di sperimentare, migliorare e studiare i seguenti campi:

## Game Mechanics
Sperimentare e implementare meccaniche di gioco sperimentando abilità di soluzione ai problemi, game design, game programming.
![cover](https://raw.githubusercontent.com/RayCatcherS/MatrixFactory/refs/heads/main/readmecover.png)
## UI Elements(UI Toolkit)
Studiare-codificare e utilizzare gli UI Elements di Unity, progettando interfacce utente personalizzate per l'editor unity. Specificamente ho scritto codice per visualizzare la struttura dati ad albero utilizzata per rappresentare i possibili percorsi dei livelli generati.
Per studiare il pacchetto UI Elements di Unity stati usati i seguenti tutorial:

### Unity Experimental GraphView API
https://www.youtube.com/watch?v=nvELzBYMK1U&list=PL0yxB6cCkoWK38XT4stSztcLueJ_kTx5f&ab_channel=IndieWafflus

### Unity Style Sheets
Fogli di stile per l'UI Toolkit

### Unity Resources/Editor Folder
Organizzare gestione delle risorse e delle cartelle del progetto.
- https://www.youtube.com/watch?v=z1wPBbK2g9o&t=119s
- https://www.youtube.com/watch?v=1O1nmZzA_EU

## Sviluppi Futuri
- Implementare tecniche di ricerca e di path pruning per aumentare l'efficienza nella generazione dei livelli
