using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;

    [SerializeField]
    TMP_Text kalanZamanTxt;


    [SerializeField]
    TMP_Text skorTxt;



    public int kalanZaman = 5;

    [SerializeField]
    GameObject turSonucPanel;

    public bool turBittimi;

    [HideInInspector]
    public int gecerliPuan;


    [SerializeField]
    GameObject pausePanel;


    Board board;

    public string anamenu;

    private void Awake()
    {
        instance = this;


        board = Object.FindObjectOfType<Board>();
    }


    private void Start()
    {
        turBittimi = false;

        StartCoroutine(GeriSayRouine());
    }

    IEnumerator GeriSayRouine()
    {
        while (kalanZaman > 0)
        {
            yield return new WaitForSeconds(1f);

            kalanZamanTxt.text = kalanZaman.ToString() + " s";
            kalanZaman--;

            if (kalanZaman <= 0)
            {

                SoundManager.instance.OyunBittiSesiCikar();
                turBittimi = true;
                turSonucPanel.SetActive(true);
            }
        }
    }

    public void PuaniArtir(int gelenPuan)
    {
        gecerliPuan += gelenPuan;
        skorTxt.text = gecerliPuan.ToString() + " Puan";
    }

    public void Karistir()
    {
        board.BoardKaristir();
    }

    public void OyunuDurdurAc()
    {
        if (!pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }


    public void AnaMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(anamenu);
    }

    public void OyunaDon()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(anamenu);
    }
}
