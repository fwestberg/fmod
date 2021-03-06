class: center, middle, title

# FMOD + Unity

???
- v2.0 er lige udkommet
- Udleverede komponenter er baseret på v1.10

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

# Tilgang

???
De fleste funktioner bliver vist udført på 2 måder

---

class: center, middle, title
background-image: url(https://media.giphy.com/media/Mp4hQy51LjY6A/giphy.gif)

# Den nemme måde

---

class: center, middle, title
background-image: url(https://media.giphy.com/media/Ty9Sg8oHghPWg/giphy.gif)

# Den fede måde

???
Som udgangspunkt er den nemme måde at foretrække til det meste

---

# Opsætning

Du skal bruge 3 ting:

1. Unity
2. FMOD Studio
3. FMOD Studio Unity Integration

Sidstnævnte importeres via *Assets > Import Package > Custom Package...*

Herefter tilføjes `FMOD Studio Listener` din character controller

???
- [Detaljeret gennemgang](https://fmod.com/resources/documentation-unity?version=2.0&page=user-guide.html)
- Generel installation af pakken og hvordan man overtager lyd outputtet

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

<div class="subject">{{name}}</div>

## Den nemme måde
Loades automatisk, når spillet startes:

*FMOD > Edit Settings > Initialization > Load Banks > __All__*

*Load Bank Sample Data* kan aktiveres for at reducere latency ved afspilning

---

name: Sound banks

<div class="subject">{{name}}</div>

## Den fede måde
*(der dog er overkill for de fleste)*

`Studio Bank Loader` tilføjes GameObject

[Kan også styres via scripts](https://www.fmod.com/resources/documentation-unity?version=2.0&page=game-components.html#loading-and-unloading-from-script)

???
- Som udgangspunkt er den nemme måde klart at foretrække
- Hvis man løber ind i problemer med memory, kan man kigge på den fede måde

---

class: center, middle, title
name: Events
# {{name}}

---

name: Events

<div class="subject">{{name}}</div>

## Den nemme måde
`Studio Event Emitter` tilføjes GameObject – og det fungerer rigtig fint til de fleste ting

[Kan også startes og stoppes via kode](https://www.fmod.com/resources/documentation-unity?version=2.0&page=game-components.html#starting-and-stopping-from-script)

---

name: Events

<div class="subject">{{name}}</div>

## Den fede måde
Hvis vi vil arbejde dynamisk med events, skal vi bruge en reference:
```
using FMODUnity;
...
[EventRef]
public string variableName
```

![FMOD EventRef UI](img/eventref-ui.png)

???
Hvordan man starter og stopper et event

---

class: center, middle, title
name: Parametre

# {{name}}

???
Hvordan man sender og opdaterer et parameter til et event
---

name: Parametre

<div class="subject">{{name}}</div>

## Den nemme måde

`Studio Parameter Trigger` tilføjes GameObject

Dette fungerer dog ikke til glidende ændringer – der skal vi have fat i...

---

name: Parametre

<div class="subject">{{name}}</div>

## Den fede måde

```
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

---

name: Parametre

<div class="subject">{{name}}</div>

## Den fede måde

Hvis vi har brug for 2-vejs kommunikation, skal vi være endnu smartere:

```
using FMOD.Studio;
...
EventInstance playerState;
```

```
private ParameterInstance speedParameter;
```

Læg mærke til `out`, der betyder at funktionen gemmer data i denne variabel

???
FMOD 2.0 har også en `Studio Global Parameter Trigger`

---

class: center, middle, title
name: Snapshots

# {{name}}

---

name: Snapshots

<div class="subject">{{name}}</div>

## Den nemme måde

1. Opret et GameObject med en `Box Collider`
2. Aktivér *Is Trigger*
3. Placér objektet i din scene
4. Tilføj en `Studio Event Emitter` og brug dette til at aktivere/deaktivere dit snapshot

???
- Hvordan man starter og stopper et mixer snapshot
- [Creating a reverb zone](https://fmod.com/resources/documentation-unity?version=2.0&page=user-guide.html#creating-a-reverb-zone)
- [Overriding vs. blending snapshots](https://qa.fmod.com/t/overriding-vs-blending-snapshots/11877/4?u=fwestberg)

---

class: center, middle, img-full
name: Snapshots

<div class="subject">{{name}}</div>

![Reverb Zone - Step 1](/img/reverbZone01.gif)

---

class: center, middle, img-full
name: Snapshots

<div class="subject">{{name}}</div>

![Reverb Zone - Step 2](/img/reverbZone02.gif)

---

class: center, middle, img-full
name: Snapshots

<div class="subject">{{name}}</div>

![Reverb Zone - Step 3](/img/reverbZone03.gif)

---

name: Snapshots

<div class="subject">{{name}}</div>

## Den fede måde

Snapshots betragtes som events, når de skal kaldes via kode:

```
EventInstance inGamePause = FMODUnity.RuntimeManager.CreateInstance("snapshot:/IngamePause");
inGamePause.start();
```

Alternativt kan man starte et almindeligt event, der indeholder et snapshot instrument

???
- Intensity kan f.eks. smoothes vha. AHDSR
- Snapshot instruments giver en anden form for fleksibilitet – mulighed for afspilning af lyde inden/efter f.eks.

---

class: center, middle, title
name: Mixing

# {{name}}

---

name: Mixing

<div class="subject">{{name}}</div>

## Live update

Når der skal mixes, er *Live Update* din ven:

1. Start spil i Unity [ *CMD+P* ]
2. Start Live Update i FMOD Studio [ *F5* ]
3. Lav ændringer
4. Build [ *F7* ]

???
- [Using live update](https://fmod.com/resources/documentation-unity?version=2.0&page=user-guide.html#connect-using-live-update)

---

class: center, middle, title
name: Nice to know

# {{name}}

---

name: Nice to know

<div class="subject">{{name}}</div>

# Hvordan laver man et rytmisk spil?

*eller - hvordan hiver jeg info om tempo og takt ud af FMOD?*

---

name: Nice to know

<div class="subject">{{name}}</div>

# Hvordan laver jeg en stealth-effekt?

Brug et mixer snapshot til at dæmpe og forstærke lyde

---

name: Nice to know

<div class="subject">{{name}}</div>

# Hvad med version control?

Hvis man skal arbejde med version control (som regel git) i et team, er det bedst at køre et single/multiple platform build og kun dele indholdet af *Build*-mappen. Alle i teamet skal have adgang til dette, så det er en god idé at lægge det som en mappe i Unity-projektet, der kan refereres med et relativt link.

Nedenstående tilføjes *.gitignore*:

```
Assets/FMODStudioCache.asset
Assets/StreamingAssets/*.bank
fmod.log
fmod_editor.log
```

---

name: Nice to know

<div class="subject">{{name}}</div>

# Hvad med FFT og så'n?

???
- Lytte til lyd-events og gøre ting baseret på amplitude eller frekvens
