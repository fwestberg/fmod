class: center, middle, title

# FMOD + Unity

---

class: center, middle

*"Hvordan integrerer man FMOD i Unity? Sound banks, parametre, mixer snapshots mm. Den sidste del af undervisningen vil gå med mulighed for individuel hjælp til opstart af eget projekt."*

---

# Agenda
## Mandag

1. Opsætning
2. Gennemgang af funktioner

---

# Agenda
## Tirsdag

1. Opsamling
2. Implementation i egne projekter
3. Evt. hjælp til andre programmeringsproblemer

---
class: center, middle, title
background-image: url(https://media.giphy.com/media/Mp4hQy51LjY6A/giphy.gif)

# Den nemme måde

???
De fleste funktioner bliver vist udført på 2 måder

---

class: center, middle, title
background-image: url(https://media.giphy.com/media/Ty9Sg8oHghPWg/giphy.gif)

# Den fede måde

---

# Opsætning

Du skal bruge 3 ting:

1. Unity
2. FMOD Studio
3. FMOD Studio Unity Integration

Sidstnævnte importeres via *Assets > Import Package > Custom Package...*

Herefter tilføjes `FMOD Studio Listener` din character controller

???
Generel installation af pakken og hvordan man overtager lyd outputtet

---

class: center, middle, title
name: Komponenter
# {{name}}

---

# Komponenter
FMOD inkluderer en del komponenter, vi kan bruge:

- `Studio Listener`
- `Studio Bank Loader`
- `Studio Event Emitter`
- `Studio Parameter Trigger`

???
Den nemme måde at gøre det på
[FMOD Learn](https://www.fmod.com/resources/documentation-unity?version=2.0&page=game-components.html)

---
class: center, middle, title
name: Sound banks
# {{name}}

???
- Sound banks samler alle nødvendige filer fra FMOD Studio, der skal bruges i Unity
- Til små projekter er det nok med en *Master bank*
- Til større kan man gruppere lyde i forskellige banks og loade/unloade dynamisk (for at spare hukommelse) – f.eks. ved sceneskift
- *Strings bank* skal loades, hvis events skal lokaliseres via filsti – i stedet for via GUID (globally unique identifier)

---
name: Sound banks

## Den nemme måde
Loades automatisk, når spillet startes:

*FMOD > Edit Settings > Initialization > Load Banks > __All__*

*Load Bank Sample Data* kan aktiveres for at reducere latency ved afspilning

<div class="subject">{{name}}</div>

---
name: Sound banks

## Den fede måde
*(der dog er overkill for de fleste)*

`Studio Bank Loader` tilføjes GameObject

[Kan også styres via scripts](https://www.fmod.com/resources/documentation-unity?version=2.0&page=game-components.html#loading-and-unloading-from-script)

<div class="subject">{{name}}</div>

???
- Som udgangspunkt er den nemme måde klart at foretrække
- Hvis man løber ind i problemer med memory, kan man kigge på den fede måde

---

class: center, middle, title
name: Events
# {{name}}

---

## Den nemme måde
`Studio Event Emitter` tilføjes GameObject – og det fungerer rigtig fint til de fleste ting

[Kan også startes og stoppes via kode](https://www.fmod.com/resources/documentation-unity?version=2.0&page=game-components.html#starting-and-stopping-from-script)

<div class="subject">{{name}}</div>

---

## Den fede måde
Hvis vi vil arbejde dynamisk med events, skal vi bruge en reference:
```cs
[EventRef]
public string variableName
```

<div class="subject">{{name}}</div>

???
Hvordan man starter og stopper et event

---
class: center, middle, title
# Parametre

???
Hvordan man sender og opdaterer et parameter til et event
---
# Parametre
## Den nemme måde

`Studio Parameter Trigger` tilføjes GameObject

Dette fungerer dog ikke til glidende ændringer – der skal vi have fat i...

---
# Parametre
## Den fede måde

```cs
FMODUnity.StudioEventEmitter emitter;
void OnEnable()
{
    var target = GameObject.Find("NameOfEmitterObject");
    emitter = target.GetComponent<FMODUnity.StudioEventEmitter>();
}
void Update()
{
    float value = 1.0f; // calculate the value every frame
    emitter.SetParameter("ParameterName", value);
}
```

Hvis vi har brug for 2-vejs kommunikation, skal vi være endnu smartere:

```cs
FMOD.Studio.EventInstance playerState;
```

```cs
private ParameterInstance speedParameter;
```

Læg mærke til `out`, der betyder at funktionen gemmer data i denne variabel

???
FMOD 2.0 har også en `Studio Global Parameter Trigger`

---
class: center, middle, title
# Snapshots
---
# Snapshots
Hvordan man starter og stopper et mixer snapshot

Alternativt kan man starte et almindeligt event, der indeholder et mixer snapshot

---

# Mixing

Når der skal mixes, er *Live Update* din ven:

1. Start spil i Unity [ *CMD+P* ]
2. Start Live Update i FMOD Studio (nederst i vinduet)
3. Lav ændringer
4. Build [ *F7* ]

---

# Nice to know

- Hvis man skal arbejde med version control (som regel git) i et team, er det bedre at køre et single/multiple platform build og kun dele indholdet af *Build*-mappen
- Lytte til lyd-events og gøre ting baseret på amplitude eller frekvens

---

# Forespørgsler

- Rytmisk spil (info om BPM og underdelinger)
- Avanceret anvendelse af spatializer (stealth)