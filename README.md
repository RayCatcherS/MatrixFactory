# MatrixFactory
Matrix Factory è un gioco-esperimento sviluppato nel tempo libero in circa 4 mesi. Nel gioco si devono comporre i giusti path per portare in consegna la quantità sufficiente di pacchi per completare l'ordine. I livelli sono stati generati usando un approccio di ricerca di tutte le soluzioni, ovvero ricerca in profondità (Depth-First Search, DFS). I livelli generati vengono poi valutati da un algoritmo che ne calcola uno scoring della difficoltà rispetto alla complessità del path del livello(Lunghezza path, cambi di direzione, distanza tra partenza e arrivo). I livelli vengono poi presentati al giocatore in ordine di difficoltà(ranking). Tutti gli assets, modelli 3D, UI e livelli di gioco sono stati realizzati da me.

L'obiettivo di questo progetto è stato quello di sperimentare, migliorare e studiare i seguenti campi:

### Game Mechanics
Sperimentare e implementare meccaniche di gioco sperimentando abilità di soluzione ai problemi, game design, game programming.

### UI Elements(UI Toolkit)
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

## Futuri miglioramenti
- Implementare tecniche di ricerca e di path pruning per aumentare l'efficienza nella generazione dei livelli
