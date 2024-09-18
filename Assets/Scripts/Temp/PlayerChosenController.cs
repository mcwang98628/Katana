using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChosenController : MonoBehaviour
{
    public Animator _animator;
    public GameObject UpGradeParticles;
    public FeedBackObject UpGradeFeedback;
    public AudioClip UpGradeAudio;
    public AudioClip GameStartAudio;

    private void OnGUI()
    {
        if(GUI.Button(new Rect(0,0,100,50),"UpGrade"))
        {
            UpgradeFeedback();
        }
        if(GUI.Button(new Rect(0,100,100,50),"EnterGame"))
        {
            GameStartFeedback();
        }
    }
    public void UpgradeFeedback()
    {
        //if(UpGradeFeedback!=null)
        //{
        //    FeedbackManager.Inst.UseFeedBack(GetComponent<RoleController>(),UpGradeFeedback);
        //}
        if (UpGradeParticles != null)
        {
            UpGradeParticles.DuplicateAtPosition(transform.position);
        }
        if (UpGradeAudio != null)
        {
            UpGradeAudio.Play();
        }
        _animator.SetTrigger("UpGrade");
    }
    public void GameStartFeedback()
    {
        if (GameStartAudio != null)
        {
            GameStartAudio.Play();
        }

        _animator.SetTrigger("EnterGame");
    }


}
