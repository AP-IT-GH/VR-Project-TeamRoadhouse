# Tutorial
# VR Setup
## 1. Packages
Om te starten met het VR project hebben we een VR starter project gebruikt van Unity dat al enkele configuraties heeft die we nodig zullen hebben.

Als je een VR project van scratch wilt beginnen zul je de volgende packages moeten installeren:
- XR Plugin Management
- XR Interaction Toolkit
- OpenXR Plugin
- Universal RP

![image](https://user-images.githubusercontent.com/61287853/172693262-69e2fd6d-b66c-4c5d-add3-00391d55d212.png)
<br><br>
Ook heb je een XR Rig, XR Interaction Manager en Input Action Manager nodig, deze zullen standaard geconfigureerd zijn met het starter project.

## 2. Build settings
Ga naar Edit > Project Settings, kies hier de XR Plug-in Management pagina. 
Selecteer hier oculus van de lijst plug-in providers bij Android. Hiermee kan je je project builden als een app en testen/spelen op je oculus. 
<br><br>
![image](https://user-images.githubusercontent.com/61287853/172693465-44a3221e-41f8-40bf-8410-31fa6e012eb4.png)
<br><br>
Bij File > Build settings hier moet je ook van platform veranderen naar Android, dit kan wel enkel wanneer je de Android Export module hebt geïnstalleerd bij de installatie van de Unity Editor.

## 3. Movement
Voor het movement voegen we een Locomotion System toe aan de XR Rig
<br><br>
![image](https://user-images.githubusercontent.com/61287853/172693640-f6a9d1b1-87ba-4122-ab41-0bc50a4f96e7.png)
<br><br>
Voeg een nieuwe Snap Turn Provider component (Action-based) toe aan het Locomotion System en zet de Left hand Snap Turn Action uit, zo kan je enkel je hoofd draaien met de rechter controller. De linker controller gaan we gebruiken om te bewegen. 
<br><br>

Je kan de Turn Amount en Debounce Time aanpassen naar je eigen wil. De Debounce Time is hoe lang er tussen elke draai zit.
<br><br>
![image](https://user-images.githubusercontent.com/61287853/172693797-6621f75a-8521-4b9e-8dfa-a142e868fb5a.png)
<br><br>

Voor te bewegen gaan we een Continuous Move Provider (Action-based) gebruiken, dit voegen we ook toe aan het Locomotion System. 
![image](https://user-images.githubusercontent.com/61287853/172694046-e17f3547-9b23-4197-b4e0-428825dedfff.png)
<br><br>
Hier zetten we de Right Hand Move Action uit om de linker controller te kunnen gebruiken om je movement te controleren. Je kan de Move Speed aanpassen naar je eigen voorkeur.

<br><br>
Aan de XR Rig voegen we dan nog een Character Controller component toe, dit heeft een collision box en geeft ons de mogelijkheid te interageren met de speelwereld.
Om de Character Controller juist te laten werken voeg je ook nog de Character Controller Driver toe waar je de Min. en Max. hoogte van je character kan bepalen.
<br><br>
![image](https://user-images.githubusercontent.com/61287853/172694167-91f7109b-af31-4be2-8ef7-1f64293b3fb4.png)

## 4. Grijpbare objecten
In de hiërarchie kies XR Rig > Camera Offset > LeftHand Controller en kies hier je Model Prefab voor je linkerhand. Doe hetzelfde voor het rechterhand.
<br><br>
![image](https://user-images.githubusercontent.com/61287853/172694560-d1376e75-16e7-4498-a568-ececa5bb003c.png)
<br><br>
Voor enkele minigames moet je objecten kunnen vastpakken, hiervoor moeten de objecten een XR Grab Interactable hebben. Voeg deze component toe aan het object dat je wilt vastgrijpen.
<br><br>
![image](https://user-images.githubusercontent.com/61287853/172694647-dee6909a-f4c3-492f-8b65-ea8281050715.png)
<br><br>
Om het object deftig te kunnen gooien zal je enkele aanpassingen moeten doen aan het object.
Om te voorkomen dat je object door te grond valt zet je de Collision Detection op Continuous Dynamic, kijk zeker ook dat de movement type op Kinematic staat om de physics van het object 
<br><br>
![image](https://user-images.githubusercontent.com/61287853/172694752-a51f986c-d45e-4283-a4a7-e9f25994645b.png)

# ML Agent 
Eerst maken we een nieuw Unity project aan, we kiezen gewoon 3D als starter project.
Via de package manager installeren we de ML-Agents unity package: 
<br><br>
![image](https://user-images.githubusercontent.com/61287853/172677267-4c2709a3-d05d-4850-a919-e74c920b150b.png)

<br><br>

Nu maken we een simpele scene, we hebben een (Seeker)Agent nodig (rode cubus in ons geval), een Target/player (die later in de code spauwnt wordt), een platform,
en muren(walls) om ons domein af te bakennen.
Deze items steken we in een leeg gameObject, trainingArea, waar onze agent zal trainen.
<br><br>
![image](https://user-images.githubusercontent.com/61287853/172679423-333c8445-26d3-442b-884e-c06e35f2d53b.png)
<br><br>

Vervolgens voegen we paar nodige components toe aan de Agent: 
<br><br>
![image](https://user-images.githubusercontent.com/61287853/172679863-d6cde72d-6f93-452d-9f16-8323a77b4fa4.png)
![image](https://user-images.githubusercontent.com/61287853/172680359-49765d7c-4089-498b-b7a3-b648f9a5ac7e.png)
![image](https://user-images.githubusercontent.com/61287853/172680424-9531e474-3ef3-45ed-90d8-6c134ad9a509.png)

<br><br>
Vervolgens maken we een nieuwe script aan voor onze Agent, deze laten we overerven door Agent en crreeren/overriden we volgende functies en variables:
<br><br>

```csharp
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float rotationSpeed = 350;
    [SerializeField] private MonitorTool monitorTool;
    [SerializeField] private float maxTime = 60f;
    
    private Rigidbody rb;
    private Environment env;
    private float timer = 0f;

    public override void Initialize(){}

    private void Update(){}

    public override void OnEpisodeBegin(){}

    public override void CollectObservations(VectorSensor sensor){}

    public override void OnActionReceived(ActionBuffers actions){}

    void OnCollisionEnter(Collision collision){}

    public override void Heuristic(in ActionBuffers actionBuffers){}
```
in de Initialize functie initalizeren we de volgende velden :
```csharp
  public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        env = GetComponentInParent<Environment>();
        timer = maxTime; // Reset timer
    }

```
alle methodes hebben in commentaar een beetje uitleg met wat de functie percies verwezelijkt:
Update Methode
```csharp
 private void Update()
    {
        // If agent falls, give negative reward and end episode
        if(transform.position.y < transform.parent.position.y - 3)
        {
            // in de monitorTool (canvas) count fails:
            monitorTool.FailsCount += 1;
            EndEpisode();
        }

        // Create timer to give the agent a maximum time to find the player
        if(timer <= 0f)
        {
            // in de monitorTool (canvas) count fails:
            monitorTool.FailsCount += 1;
            EndEpisode();
        }
        timer -= Time.deltaTime; // Take time elapsed from timer
    }
```
OnEpisodeBegin metode :
```csharp
    public override void OnEpisodeBegin()
    {
        // in de monitorTool (canvas) count episodes:
        monitorTool.EpisodesCount += 1;

        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        
        // resets the timer
        timer = maxTime;

        // resets enviroment with new positions for player and seeker
        env.ResetEnvironment();
    }
```
CollectObservations methode :
```csharp
  public override void CollectObservations(VectorSensor sensor)
    {
        // position of Agent takes 3 observations (x, y, z)
        sensor.AddObservation(transform.position); 
        // rotation of Agent takes 4 observations (x, y, z, w)
        sensor.AddObservation(transform.rotation);
        // position of target takes 3 observations (x, y, z)
        sensor.AddObservation(env.players[0].transform.position);  
    }
```
OnActionReceived methode :
```csharp
 public override void OnActionReceived(ActionBuffers actions)
    {
        var vectorAction = actions.DiscreteActions;

        // Add negative reward when agent doesn't move
        if (vectorAction[0] == 0 && vectorAction[1] == 0)
        {
            notMoved = true;
            return;
        }
        else
            notMoved = false;
        
        // 0 = IDLY , 1 = BACKWARDS , 2 = FORWARDS
        if (vectorAction[0] != 0)
        {
            Vector3 translation = transform.forward * moveSpeed * (vectorAction[0] * 2 - 3) * Time.deltaTime;
            transform.Translate(translation, Space.World);
        }

        // 0 = IDLY , 1 = LEFT , 2 = RIGHT
        if (vectorAction[1] != 0) 
        {
            float rotation = rotationSpeed * (vectorAction[1] * 2 - 3) * Time.deltaTime;
            transform.Rotate(0, rotation, 0);
        }
    }
```
OnCollisionEnter Methode :
```csharp
 void OnCollisionEnter(Collision collision)
    {
        // Stop Episode when Agent finds player - SET REWARD TO 10
        if (collision.transform.CompareTag("Player"))
        {
            SetReward(1f);
            monitorTool.SuccesCount += 1;
            EndEpisode();
        }
    }
```

Heuristic metode : deze methode is voor het testen/het doen van imitation learning
```csharp 
 public override void Heuristic(in ActionBuffers actionBuffers)
    {
        base.Heuristic(actionBuffers);

        var outputAction = actionBuffers.DiscreteActions;
        outputAction[0] = 0;
        outputAction[1] = 0;

        if (Input.GetKey(KeyCode.UpArrow)) // Moving forwards
            outputAction[0] = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) // Turning left
            outputAction[1] = 1;
        else if (Input.GetKey(KeyCode.RightArrow)) // Turning right
            outputAction[1] = 2;
    }
```

dit was alles omtrent de agent nu moeten we nog een Monitor Tool maken die de episodes,geslaage,gefaalde runs bij houdt zodat we een visueel beeld krijgen van hoe goed de agent aan het presteren is.
creer een script MonitorTool.
```csharp
public class MonitorTool : MonoBehaviour
{
    [SerializeField] private Text episodesText;
    [SerializeField] private Text succesText;
    [SerializeField] private Text failsText;

    private int episodesCount = -1;
    public int EpisodesCount
    {
        get { return episodesCount; }
        set { 
            episodesCount = value;
            episodesText.text = $"Episodes: {episodesCount}";
        }
    }

    private int succesCount;
    public int SuccesCount
    {
        get { return succesCount; }
        set
        {
            succesCount = value;
            succesText.text = $"Succes: {succesCount}";
        }
    }

    private int failsCount;
    public int FailsCount
    {
        get { return failsCount; }
        set
        {
            failsCount = value;
            failsText.text = $"Fails: {failsCount}";
        }
    }
}
```

Enviroment script wordt gebruikt om de trainingArea telkens ramdom te creeren 
```csharp
public class Environment : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform agent;

    [SerializeField] private int targets;

    public List<GameObject> players = new List<GameObject>();

    public void ResetEnvironment()
    {
        // Remove all players still in the field
        foreach(var player in players)
        {
            Destroy(player.gameObject);
        }
        players.Clear();

        // Reset agent position
        do
        {
            agent.transform.localPosition = RandomPosition(0.5f);
            agent.transform.localEulerAngles = RandomRotation();
        } while (Physics.CheckSphere(agent.transform.localPosition, .1f));

        // Reset player (Agent's target) position
        for (int i = 0; i < targets; i++)
        {
            GameObject player = Instantiate(playerPrefab, transform.parent);
            player.transform.parent = transform;
            players.Add(player);
            do
            {
                player.transform.localPosition = RandomPosition(player.transform.localPosition.y);
            } while (Physics.CheckSphere(player.transform.localPosition, .1f));
        }
    }

    public Vector3 RandomPosition(float y)
    {
        float x = Random.Range(-14.25f, 14.25f);
        float z = Random.Range(-14.25f, 14.25f);

        return new Vector3(x, y, z);
    }

    private Vector3 RandomRotation()
    {
        float y = Random.Range(0.0f, 360.0f);

        return new Vector3(0f, y, 0f);
    }
}
```
Nu even terug naar de scene, we creeren nu een eventsystem.
<br><br>
![image](https://user-images.githubusercontent.com/61287853/172687691-9c8667c5-bf18-47f2-a17c-3d0c430bbc1c.png)
<br><br>
we maken nu ook een nieuw canvas aan waar we 3 text elementen op plaatsen.
<br><br>
![image](https://user-images.githubusercontent.com/61287853/172687936-06f93af0-eeb2-439b-98b3-262169968709.png)
<br><br>
daarna plaatsen we het monitor tool script op deze canvas.
<br><br>
![image](https://user-images.githubusercontent.com/61287853/172688143-1a9da067-532f-4c9e-96b2-49d573d2f016.png)

als je al deze stappen hebt gevolgd dan is de unity setup compleet.
Nu zullen we da .yaml file configureren voor onze agent.
eerst maak je een config folder aan in de assets folder
<br><br>
![image](https://user-images.githubusercontent.com/61287853/172689999-48eb27c2-cce3-4228-a4ae-a848d88b424c.png)
<br><br>
Agent.yaml file ziet er zo uit. de in commenaar gezette lijnen worden gebruikt voor Imitation Learning.
Als je aan Imitation Learning wilt doen zal je in het component "Demostration Recorder" eerst een imitatie moeten opnemen met Heuristic voor dat je deze velden kan gebruiken.
```yaml
behaviors:
  SeekerAgent:
    trainer_type: ppo
    hyperparameters:
      batch_size: 256 
      buffer_size: 1000000
      learning_rate: 3.0e-4
      beta: 5.0e-4
      epsilon: 0.2
      lambd: 0.99
      num_epoch: 3
      learning_rate_schedule: linear
      beta_schedule: constant
      epsilon_schedule: linear
    network_settings:
      normalize: false
      hidden_units: 128
      num_layers: 2
    reward_signals:
      extrinsic:
        gamma: 0.99
        strength: 1.0
      curiosity:
        strength: 0.3
        gamma: 0.99
        encoding_size: 256
        learning_rate : 1e-3
      gail:
        strength: 0.7
        demo_path: D:\Unity projects\VR-Experience\VR-Slasher\ImitationModels\FinalDemo.demo ## Dit moet veranderd worden naar de path van je demo
    max_steps: 2000000
    time_horizon: 64
    summary_freq: 2000
```
De agent gebruikt imitationLearning, we moeten dus ook een playerDemo opnemen waarvan de agent kan leren.

Gebruik hiervoor de DemonstrationRecorder component op je agent.

Als je al deze stappen hebt overlopen ben je klaar om je Agent te gaan trainen.
doormiddel van anaconda open het project in de config map en run dit command.

```
mlagents-learn Agent.yaml --run-id=AgentV1
```
*Zorg dat je deze command in de config folder runt (dezelfde folder, waar je Agent.yaml zich bevindt)*

# Sneller trainen
Trainen via de unity editor verliep heel traag bij mij, indien dit ook het geval is, of indien je gewoonweg de agent wat sneller wilt trainen kan dit op volgende manier:

Maak een build van je unity project, zorg dat je de ai-training scene kiest als scene die gebuild moet worden.
Daarna kan je met volgende command de ai laten trainen, zonder dat het visueel gerendered moet worden, dit gaat een pak sneller.
```
mlagents-learn Assets/ai-config/Agent.yaml --env=ai-build --run-id=FinalInTrainingArea --force --no-graphics
```
