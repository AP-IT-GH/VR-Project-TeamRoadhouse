# Tutorial
# VR experience
# ML Agent 
Eerst maken we een nieuw Unity project aan, we kiezen gewoon 3D als starter project.
Via de package manager installeren we de ML-Agents unity package: 
<br><br>
![image](https://user-images.githubusercontent.com/61287853/172677267-4c2709a3-d05d-4850-a919-e74c920b150b.png)


<br>
<br>

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
    [SerializeField] private float maxTimeNotMoved = 3f;
    private bool Collided = false;
    private bool notMoved = false;
    
    private Rigidbody rb;
    private Environment env;
    private float timer = 0f;
    private float movedTimer = 0f;

    public override void Initialize(){}

    private void Update(){}

    public override void OnEpisodeBegin(){}

    public override void CollectObservations(VectorSensor sensor){}

    public override void OnActionReceived(ActionBuffers actions){}

    void OnCollisionEnter(Collision collision){}
 
    private void OnCollisionExit(Collision other){}

    public override void Heuristic(in ActionBuffers actionBuffers){}
```
in de Initialize functie initalizeren we de volgende velden :
```csharp
  public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        env = GetComponentInParent<Environment>();
        timer = maxTime;
        movedTimer = maxTimeNotMoved;
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
            SetReward(-1f);
            monitorTool.FailsCount += 1;
            EndEpisode();
        }

        // Create timer to give the agent a maximum time to find the player
        if(timer <= 0f)
        {
            SetReward(-1f);
            monitorTool.FailsCount += 1;
            EndEpisode();
            timer = maxTime; // Reset timer
        }
        timer -= Time.deltaTime; // Take time elapsed from timer

        // if the player hits hits
        if (Collided)
        {
            print("hitting wall");
            SetReward(-0.5f);
        }

        if (notMoved)
        {
            movedTimer -= Time.deltaTime;
            if (movedTimer <= 0)
            {
                print("not moved int 3 sec");
                SetReward(-0.3f);
            }
        }
        else
            movedTimer = maxTimeNotMoved;
        
    }
```
OnEpisodeBegin metode :
```csharp
    public override void OnEpisodeBegin()
    {
        // in de monitorTool (canvas) counts episodes:
        monitorTool.EpisodesCount += 1;

        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;

        // resets enviroment with new positions for player and seeker
        env.ResetEnvironment();
    }
```
CollectObservations methode :
```csharp
  public override void CollectObservations(VectorSensor sensor)
    {
        //position of Agent takes 3 observations (forward,backward, idle)
        sensor.AddObservation(transform.position); 
         //rotation of Agent takes 4 observations 
        sensor.AddObservation(transform.rotation);
        //position  of target takes 3 observations 
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
        if (collision.gameObject.CompareTag("Collidable"))
        {
            Collided = true;
        }
    }

```
OnCollisionExit methode :
```csharp
   private void OnCollisionExit(Collision other)
    {
        Collided = false;
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
            outputAction[0] = 2;
        else if (Input.GetKey(KeyCode.DownArrow)) // Moving backwards
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
Player script
```csharp
public class Player : MonoBehaviour
{
    private Environment environment;

    private void Start()
    {
        environment = GetComponentInParent<Environment>();
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
eerst maak je een config file in de assets folder
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
      batch_size: 10
      buffer_size: 100
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
        strength: 0.1
        gamma: 0.99
        encoding_size: 256
        learning_rate : 1e-3
      # gail:
      #   strength: 0.5
      #   demo_path: ImitationModels/SeekerAgentImati.demo
    # behavioral_cloning:
    #   strength: 0.5
    #   demo_path: D:\Unity projects\VR-Experience\VR-Slasher\ImitationModels\SeekerAgentImati.demo
    max_steps: 2000000
    time_horizon: 64
    summary_freq: 2000
   ```
   Als je al deze stappen hebt overlopen ben je klaar om je Agent te gaan trainen.
   doormiddel van anaconda open het project in de config map en run dit command
```
mlagents-learn Agent.yaml --run-id=AgentV1
```
   


