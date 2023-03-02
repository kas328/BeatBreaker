using UnityEngine;

public class DeadZone : MonoBehaviour
{
    //float curtime;
    //private void Update()
    //{
    //    curtime += Time.deltaTime;
    //}
    private void OnTriggerEnter(Collider other)
    {
        LayerMask beat = 1 << 10;
        GameObject note = other.gameObject;
        if (((1 << note.layer) & beat) == 0) return;

        NoteControl noteControl = note.GetComponent<NoteControl>();
        if (!noteControl.IsHit)
        {
            ScoreManager.Instance.Missed();

            noteControl.GotHit();
        }
        //print(curtime);
    }
}