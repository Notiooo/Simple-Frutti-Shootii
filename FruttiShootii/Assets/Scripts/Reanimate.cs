using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reanimate : MonoBehaviour
{

    public bool isActive = false;

    public float stage1EffectTime = 3f; 
    private float stage1Timer;
    public float stage1FrequencyMultiplayer = 5f;
    public float stage1AmplitudeMultiplayer = 0.02f; 

    public float stage2EffectTime = 0.75f;
    private float stage2Timer;
    public float stage2FrequencyMultiplayer = 21.5f;
    public float stage2AmplitudeMultiplayer = 0.02f;

    private bool stage3 = false;
    public float stage3EffectTime = 1f;
    private float stage3Timer;


    public float stage4EffectTime = 2f;
    private float stage4Timer; 

    float baseX = 0f;
    float baseZ = 0f;

    public float frequency = 15f;
    public float amplitude = 0.001f;
    public float acceleration = 0f;

    private bool transmutation = false;
    public PlantTransmutator plantTransmutator;
    public Rigidbody rigidbody;

    public float velocity = 10f;
    private UnityEngine.AI.NavMeshAgent agent;

    private Vector3 targetPosition;
    private bool hasTarget = false;


    private float timer = 0f;
    private float interval = 2f; // Interwał czasu (10 sekund)


    void Start()
    {
        isActive = false;
        stage1Timer = stage1EffectTime;
        stage2Timer = stage2EffectTime;
        stage3Timer = stage3EffectTime;
        stage4Timer = stage4EffectTime;

        baseX = transform.position.x;
        baseZ = transform.position.z;
        rigidbody.freezeRotation = true; 
        rigidbody.isKinematic = true; 
        rigidbody.detectCollisions = false; 

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        TurnAlive();
        if(isActive){
            if(stage1Timer > 0f){
                frequency +=  stage1FrequencyMultiplayer * Time.deltaTime / stage1EffectTime;
                amplitude +=  stage1AmplitudeMultiplayer * Time.deltaTime / stage1EffectTime;
                transform.position = new Vector3(baseX + amplitude * Mathf.Sin(frequency*Time.time) , transform.position.y, baseZ + amplitude*Mathf.Sin(frequency*Time.time)); 
                stage1Timer -= Time.deltaTime;
            } else if(stage2Timer > 0f){
                frequency +=  stage2FrequencyMultiplayer*Time.deltaTime / stage2EffectTime;
                amplitude +=  stage2AmplitudeMultiplayer * Time.deltaTime / stage2EffectTime;
                transform.position = new Vector3(baseX + amplitude * Mathf.Sin(frequency*Time.time) , transform.position.y, baseZ + amplitude *Mathf.Sin(frequency*Time.time)); 
                stage2Timer -= Time.deltaTime;
            } else if(stage3 == false){
                //rigidbody.useGravity = false;
                rigidbody.isKinematic = false; 
                rigidbody.velocity = new Vector3(0f, velocity, 0f);
                stage3 = true;
            }  else if(stage3Timer > 0f){

                stage3Timer -= Time.deltaTime;
            }
            
            else if(transmutation == false){
                transmutation = true;
                rigidbody.detectCollisions = true; 
                plantTransmutator.Tansmutate();
                
            } else if(stage4Timer > 0f){

                stage4Timer -= Time.deltaTime;
            } else {
                agent.enabled = true;
            }

        }

        if(agent.enabled){
            timer += Time.deltaTime;
            if (!hasTarget || timer >= interval){
                timer = 0f;
                GetRandomPoint();
            }
            else {
                agent.SetDestination(targetPosition);
                if (agent.remainingDistance <= agent.stoppingDistance) {
                    hasTarget = false;
                }


                Vector3 direction = (targetPosition - transform.position).normalized;
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                Vector3 targetRotation = new Vector3(0, targetAngle, 0);
                transform.rotation = Quaternion.Euler(targetRotation);
            }
        }





    }


    void TurnAlive(){
        if(isActive == false){
            timer += Time.deltaTime;
            if (timer >= interval){
                timer = 0f;
                isActive = (Random.Range(0, 10) == 0);
            }
        }
    }




    void GetRandomPoint()
    {
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(Random.insideUnitSphere * 10f + transform.position, out hit, 10f, UnityEngine.AI.NavMesh.AllAreas))
        {
            targetPosition = hit.position;
            hasTarget = true;
        }
    }



}