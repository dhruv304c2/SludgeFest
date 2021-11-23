using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CapturePoint : MonoBehaviourPun
{
    [Header("Requierd Fields...")]
    [SerializeField] SpriteRenderer BoardSprite;
    [SerializeField] SpriteRenderer FrontPoleSprite;
    [SerializeField] Animator BoardAnimator;
    [SerializeField] SpriteRenderer Fill;
    [SerializeField] float AttackProgressDeltaPP = 0.01f;
    [SerializeField] float flashDuration = 0.1f;
    [SerializeField] Material Flashmaterial;
    public string ControllingTeam = "Neutral";
    [SerializeField] public Transform trackPoint;
    public string Name;

    [Header("Halos...")]
    [SerializeField] GameObject RedHalo;
    [SerializeField] GameObject BlueHalo;

    [Header("Colors...")]
    [SerializeField] Color Blue;
    [SerializeField] Color Red;
    [SerializeField] Color Neutral;

    public bool UnderAttack; 

    public string Attacker="";
    public string CurrentAttacker = "";
    public float AttackProgress = 0;
    float timer = 0f;
    Material defMaterial;

    void Start()
    {
        defMaterial = BoardSprite.material;        
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        if(timer >= 1f)
        {
            CurrentAttacker = "";
            UnderAttack = false;
            timer = 0f;
        }
        
        switch (ControllingTeam)
        {
            case "Neutral":

                BlueHalo.SetActive(false);
                RedHalo.SetActive(false);
                break;
            
            case "Red":

                BlueHalo.SetActive(false);
                RedHalo.SetActive(true);
                BoardAnimator.Play("Red", 0);
                break;

            case "Blue":

                BlueHalo.SetActive(true);
                RedHalo.SetActive(false);
                BoardAnimator.Play("Blue", 0);
                break;
            
            default :
                BlueHalo.SetActive(false);
                RedHalo.SetActive(false);
                break;
        }
        switch (Attacker)
        {
            case "Neutral":

                Fill.color = new Color(Neutral.r,Neutral.g,Neutral.b,Fill.color.a);
                break;

            case "Red":

                Fill.color = new Color(Red.r, Red.g, Red.b,Fill.color.a);
                break;

            case "Blue":

                Fill.color = new Color(Blue.r, Blue.g, Blue.b,Fill.color.a);
                break;

            default:

                Fill.color = new Color(Neutral.r, Neutral.g, Neutral.b,Fill.color.a);
                break;
        }
    }

    public void AttackedByTeam(string AttackerTeam)
    {
        photonView.RPC("AttackByTeamOnNetwork", RpcTarget.All, AttackerTeam);
    }

    [PunRPC] public void AttackByTeamOnNetwork(string AttackerTeam)
    {

        CurrentAttacker = AttackerTeam;
        if(AttackerTeam!=ControllingTeam)
            UnderAttack = true;
            if (Attacker != AttackerTeam && AttackProgress > 0f)
            {
                AttackProgress -= AttackProgressDeltaPP;
            }
            else if (AttackProgress < 1f && AttackerTeam != ControllingTeam)
            {
                Attacker = AttackerTeam;
                ControllingTeam = "Neutral";
                AttackProgress += AttackProgressDeltaPP;

            }
            else if (AttackerTeam!=ControllingTeam)
            {
                Flash();
                ControllingTeam = AttackerTeam;
                StartCoroutine(InGameMessages.ShowMessage(AttackerTeam + " Team Captured Site " + Name, 3f));
                AttackProgress = 1f;
            switch (ControllingTeam)
            {
                case "Neutral":

                    BlueHalo.SetActive(false);
                    RedHalo.SetActive(false);
                    break;

                case "Red":

                    BlueHalo.SetActive(false);
                    RedHalo.SetActive(true);
                    RedHalo.GetComponent<Animator>().Play("HaloAnimation");
                    break;

                case "Blue":

                    BlueHalo.SetActive(true);
                    RedHalo.SetActive(false);
                    BlueHalo.GetComponent<Animator>().Play("HaloAnimation");
                    break;

                default:
                    BlueHalo.SetActive(false);
                    RedHalo.SetActive(false);
                    break;
            }
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        CurrentAttacker = "";

    }
    public void Flash()
    {
        BoardSprite.material = Flashmaterial;
        FrontPoleSprite.material = Flashmaterial;
        Invoke("ResetMaterial", flashDuration);
    }

    public void ResetMaterial()
    {
        BoardSprite.material = defMaterial;
        FrontPoleSprite.material = defMaterial;
    }

    public float GetAttackProgress()
    {
        return AttackProgress;
    }
}
