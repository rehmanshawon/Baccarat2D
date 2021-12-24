using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Cards
{
    
    // all cards
    private int[] cards;
    


    public int GetRandomCard
    {
        get
        {
            return cards[Random.Range(0, cards.Length)];
        }
    }

    public int CountOfCards
    {
        get
        {
            return cards.Length;
        }
    }

    public void ResetCards()
    {
        cards = new int[] { 101,102,103,104,105,106,107,108,109,110,111,112,113,
                            201,202,203,204,205,206,207,208,209,210,211,212,213,
                            301,302,303,304,305,306,307,308,309,310,311,312,313,
                            401,402,403,404,405,406,407,408,409,410,411,412,413 };
    }

    public void RemoveRandomCard(int CardToRemove)
    {
        int numIndex = System.Array.IndexOf(cards, CardToRemove);
        cards = cards.Where((val, idx) => idx != numIndex).ToArray();
    }
}
public class GamePlay : MonoBehaviour
{
    // sound effect
    //public AudioSource audioSource;
    // Game related stats
    public TextMeshProUGUI numberOfGames;
    public TextMeshProUGUI bankerWins;
    public TextMeshProUGUI playerWins;
    public TextMeshProUGUI tieWins;
    private int nGames = 0;
    private int nbWins = 0;
    private int npWins = 0;
    private int ntWins = 0;
    // Winner Bit - 0 means winner, 1 = player, 2 = banker, 3 = tie
    int winner = 0;
    // set in case player needs a third card
    bool drawPlayerThird = false;
    // set in case banker needs a third card
    bool drawBankerThird = false;
    // Betting Timer Control Variables
    public Image imgTimer; // Timer clock display
    public bool startTimer; // wheter to start the timer
    public bool startDeal;
    public float waitTime = 30.0f; // Starting value can be set here
    public Gradient gradient; // Gradient color changer
    public TextMeshProUGUI txtTimer; // Timer digit display
    public int countdownTime; // Here the starting value can be set

    private readonly Cards bacCards = new Cards();

    // 3 player card images
    public Image imgPC1;
    public Image imgPC2;
    public Image imgPC3;
    // 3 banker card images
    public Image imgBC1;
    public Image imgBC2;
    public Image imgBC3;
    // card back sprite
    public Sprite cardBack;
    // Card image sets sprite container array
    public Sprite[] cardImages1;
    public Sprite[] cardImages2;
    public Sprite[] cardImages3;
    public Sprite[] cardImages4;

    // number image sprites container array, two sets
    public Sprite[] playerTotalImages;
    public Sprite[] bankerTotalImages;
    // images to show total score
    public Image playerScore;
    public Image bankerScore;
    // images to show total score in winner board
    public Image playerScoreW;
    public Image bankerScoreW;
    public Image winnerLogo;
    // sprite to show who is winner
    public Sprite winnerPlayer;
    public Sprite winnerBanker;
    public Sprite winnerTie;
    //player cards total
    private int pTotal = 0;
    // banker cards total
    private int bTotal = 0;
    // player, banker two cards total
    private int p2Total = 0;
    private int b2Total = 0;
    // player, banker one card total
    private int p1Total = 0;
    private int b1Total = 0;
    // Card dealing speed and target postition
    public float speed = 1f;
    private Vector3 playerCard1InitialPosition;
    private Vector3 playerCard2InitialPosition;
    private Vector3 playerCard3InitialPosition;
    private Vector3 bankerCard1InitialPosition;
    private Vector3 bankerCard2InitialPosition;
    private Vector3 bankerCard3InitialPosition;

    private Vector3 playerCard1TargetPosition;
    public GameObject playerCard1Target;

    private Vector3 playerCard2TargetPosition;
    public GameObject playerCard2Target;

    private Vector3 playerCard3TargetPosition;
    public GameObject playerCard3Target;

    private Vector3 bankerCard1TargetPosition;
    public GameObject bankerCard1Target;

    private Vector3 bankerCard2TargetPosition;
    public GameObject bankerCard2Target;

    private Vector3 bankerCard3TargetPosition;
    public GameObject bankerCard3Target;


    public int angle;
    //public Image pcard1,pcard2,pcard3,;
    public GameObject hpc1;
    public GameObject hpc2;
    public GameObject hpc3;
    public GameObject hbc1;
    public GameObject hbc2;
    public GameObject hbc3;

    public Image ipc1, ipc2, ipc3, ibc1, ibc2, ibc3;
    private Animator DealPC1;
    private Animator DealPC2;
    private Animator DealPC3;
    private Animator DealBC1;
    private Animator DealBC2;
    private Animator DealBC3;

    private Animator wPC1;
    private Animator wPC2;
    private Animator wPC3;
    private Animator wBC1;
    private Animator wBC2;
    private Animator wBC3;
    public GameObject winnerBoard;
    private Animator wbAnimator;

    private long BetAmount = 0;
    public TextMeshProUGUI txtBetAmount;
    private int BetOn = 0;
    private string winnigText = "You have won ";
    private string losingText = "You have lost ";
    public Image winnerCrown;
    public TextMeshProUGUI resultText;
    public Button player, banker, tie,k25,k5,k1,k10,k50;
    public Text playerBet, bankerBet, tieBet;
    public Sprite playerEnabled, bankerEnabled, tieEnabled, playerDisabled, bankerDisabled, tieDisabled;
    public GameObject DealWarning;

    public Image[] scores;
    public Sprite spPlayer, spBanker, spTie;

    private int currentGameNo=0;
    private float playerBalace = 99000.0f;
    public Text txtPlayerBalance;
    // Start is called before the first frame update
    void Start()
    {
        
        
        DealPC1 = hpc1.GetComponent<Animator>();
        DealPC2 = hpc2.GetComponent<Animator>();
        DealPC3 = hpc3.GetComponent<Animator>();
        DealBC1 = hbc1.GetComponent<Animator>();
        DealBC2 = hbc2.GetComponent<Animator>();
        DealBC3 = hbc3.GetComponent<Animator>();

        wPC1 = ipc1.GetComponent<Animator>();
        wPC2 = ipc2.GetComponent<Animator>();
        wPC3 = ipc3.GetComponent<Animator>();
        wBC1 = ibc1.GetComponent<Animator>();
        wBC2 = ibc2.GetComponent<Animator>();
        wBC3 = ibc3.GetComponent<Animator>();

        wbAnimator = winnerBoard.GetComponent<Animator>();
        wbAnimator.Play("Close");

        
        startTimer = true;
        startDeal = true;
        txtTimer.text = "30";
        imgTimer.fillAmount = 1;
        imgTimer.color = gradient.Evaluate(1f);
        txtTimer.color = gradient.Evaluate(1f);
        //PrepareCards();
        bacCards.ResetCards();
        // save cards initial position
        playerCard1InitialPosition = imgPC1.transform.position;
        playerCard2InitialPosition = imgPC2.transform.position;
        playerCard3InitialPosition = imgPC3.transform.position;

        bankerCard1InitialPosition = imgBC1.transform.position;
        bankerCard2InitialPosition = imgBC2.transform.position;
        bankerCard3InitialPosition = imgBC3.transform.position;
        playerCard1TargetPosition = playerCard1Target.transform.position;
        imgPC1.gameObject.SetActive(true);
        StartCoroutine(CountDownToStart()); // Start the count down co-routine
        
    }

    // Update is called once per frame
    void Update()
    {

        
        // Run Betting timer
        if (startTimer == true)
        {
            
            //Reduce fill amount over 30 seconds
            imgTimer.fillAmount -= 1.0f / waitTime * Time.deltaTime;
            imgTimer.color = gradient.Evaluate(imgTimer.fillAmount); // change the gradient color
            txtTimer.color = gradient.Evaluate(imgTimer.fillAmount);
            if (imgTimer.fillAmount == 0)
            {
                startTimer = false; // So that the timer can be restarted in future
                OnDeal();
                DealInitialCards();
            }
                
        }
       
    }

    IEnumerator CountDownToStart()
    {
        while (countdownTime > -1)
        {
            
            txtTimer.text = countdownTime.ToString();
            yield return new WaitForSecondsRealtime(1f); // 1 second difference

            countdownTime--;
        }
        txtTimer.color = Color.white;
        yield return new WaitForSecondsRealtime(1f);  
        
    }

    public void OnBet25K()
    {
        BetAmount += 25000;
        txtBetAmount.text = BetAmount.ToString();
        player.image.sprite = playerEnabled;
        player.enabled = true;
        banker.image.sprite = bankerEnabled;
        banker.enabled = true;
        tie.image.sprite = tieEnabled;
        tie.enabled = true;
        if (BetOn == 1)
            playerBet.text = BetAmount.ToString();
        if (BetOn == 2)
            bankerBet.text = BetAmount.ToString();
        if (BetOn == 3)
            tieBet.text = BetAmount.ToString();
    }
    /*public void PlaceBet(long amount)
    {
        BetAmount = amount;
        player.image.sprite = playerEnabled;
        player.enabled = true;
        banker.image.sprite = bankerEnabled;
        banker.enabled = true;
        tie.image.sprite = tieEnabled;
        tie.enabled = true;
    }*/
    public void OnBet10K()
    {
        BetAmount += 10000;
        txtBetAmount.text = BetAmount.ToString();
        player.image.sprite = playerEnabled;
        player.enabled = true;
        banker.image.sprite = bankerEnabled;
        banker.enabled = true;
        tie.image.sprite = tieEnabled;
        tie.enabled = true;
        if (BetOn == 1)
            playerBet.text = BetAmount.ToString();
        if (BetOn == 2)
            bankerBet.text = BetAmount.ToString();
        if (BetOn == 3)
            tieBet.text = BetAmount.ToString();
    }

    public void OnBet5K()
    {
        BetAmount += 5000;
        txtBetAmount.text = BetAmount.ToString();
        player.image.sprite = playerEnabled;
        player.enabled = true;
        banker.image.sprite = bankerEnabled;
        banker.enabled = true;
        tie.image.sprite = tieEnabled;
        tie.enabled = true;
        if (BetOn == 1)
            playerBet.text = BetAmount.ToString();
        if (BetOn == 2)
            bankerBet.text = BetAmount.ToString();
        if (BetOn == 3)
            tieBet.text = BetAmount.ToString();
    }

    public void OnBet1K()
    {
        BetAmount += 1000;
        txtBetAmount.text = BetAmount.ToString();
        player.image.sprite = playerEnabled;
        player.enabled = true;
        banker.image.sprite = bankerEnabled;
        banker.enabled = true;
        tie.image.sprite = tieEnabled;
        tie.enabled = true;
        if (BetOn == 1)
            playerBet.text = BetAmount.ToString();
        if (BetOn == 2)
            bankerBet.text = BetAmount.ToString();
        if (BetOn == 3)
            tieBet.text = BetAmount.ToString();
    }

    public void OnBet50K()
    {
        BetAmount += 50000;
        txtBetAmount.text = BetAmount.ToString();
        player.image.sprite = playerEnabled;
        player.enabled = true;
        banker.image.sprite = bankerEnabled;
        banker.enabled = true;
        tie.image.sprite = tieEnabled;
        tie.enabled = true;
        if(BetOn==1)
            playerBet.text = BetAmount.ToString();
        if (BetOn == 2)
            bankerBet.text = BetAmount.ToString();
        if (BetOn == 3)
            tieBet.text = BetAmount.ToString();
    }
    public void OnBetPlayer()
    {
        BetOn = 1;
        playerBet.text = BetAmount.ToString();
        bankerBet.text = "";
        tieBet.text = "";
    }
    public void OnBetBanker()
    {
        BetOn = 2;
        bankerBet.text = BetAmount.ToString();
        playerBet.text = "";
        tieBet.text = "";
    }
    public void OnBetTie()
    {
        BetOn = 3;
        tieBet.text = BetAmount.ToString();
        playerBet.text = "";
        bankerBet.text = "";
    }

    public void OnDealWarningOK()
    {
        DealWarning.gameObject.SetActive(false);
    }
    public void OnDeal()
    {
        if (BetAmount == 0)
        {
            DealWarning.gameObject.SetActive(true);
            return;
        }
        if (BetOn == 0)
        {
            DealWarning.gameObject.SetActive(true);
            return;
        }
        player.image.sprite = playerDisabled;
        player.enabled = false;
        banker.image.sprite = bankerDisabled;
        banker.enabled = false;
        tie.image.sprite = tieDisabled;
        tie.enabled = false;
        k25.enabled = false;
        k50.enabled = false;
        k10.enabled = false;
        k1.enabled = false;
        k5.enabled = false;
        //DealInitialCards();
    }
    public void DealInitialCards()
    {
        /*playerCard1TargetPosition = playerCard1Target.transform.position;
        bankerCard1TargetPosition = bankerCard1Target.transform.position;
        playerCard2TargetPosition = playerCard2Target.transform.position;
        bankerCard2TargetPosition = bankerCard2Target.transform.position;
        playerCard3TargetPosition = playerCard3Target.transform.position;
        bankerCard3TargetPosition = bankerCard3Target.transform.position;
        imgPC1.sprite = null;
        imgPC2.sprite = null;
        imgPC3.sprite = null;
        imgBC1.sprite = null;
        imgBC2.sprite = null;
        imgBC3.sprite = null;*/
        imgPC1.sprite = cardBack;
        imgPC2.sprite = cardBack;
        imgPC3.sprite = cardBack;
        imgBC1.sprite = cardBack;
        imgBC2.sprite = cardBack;
        imgBC3.sprite = cardBack;
        ipc1.sprite = cardBack;
        ipc2.sprite = cardBack;
        ipc3.sprite = cardBack;
        ibc1.sprite = cardBack;
        ibc2.sprite = cardBack;
        ibc3.sprite = cardBack;

        wPC3.gameObject.SetActive(false);
        wBC3.gameObject.SetActive(false);
        playerScore.gameObject.SetActive(false);
        bankerScore.gameObject.SetActive(false);
        winnerBoard.gameObject.SetActive(false);
        DealPC1.Play("Shoe");
        DealPC2.Play("Shoe");
        DealPC3.Play("Shoe");
        DealBC1.Play("Shoe");
        DealBC2.Play("Shoe");
        DealBC3.Play("Shoe");
        wPC1.Play("Idle");
        wPC2.Play("Idle");
        wPC3.Play("Idle");
        wBC1.Play("Idle");
        wBC2.Play("Idle");
        wBC3.Play("Idle");
        wbAnimator.Play("Close");
        // get new card deck if necessary
        if (bacCards.CountOfCards < 6)
            bacCards.ResetCards();
        
        StartCoroutine(MoveCardsInitial());
    }
    
    IEnumerator MoveCardsInitial()
    {
        
        PrepareCards();
        yield return new WaitForSecondsRealtime(.5f);        
        DealPC1.Play("Deal");      
        yield return new WaitForSecondsRealtime(2f);
        playerScore.sprite = playerTotalImages[p1Total];
        playerScore.gameObject.SetActive(true);
        DealBC1.Play("Deal");        
        yield return new WaitForSecondsRealtime(2f);
        bankerScore.sprite = bankerTotalImages[b1Total];
        bankerScore.gameObject.SetActive(true);
        DealPC2.Play("Deal");
        yield return new WaitForSecondsRealtime(2f);
        playerScore.sprite = playerTotalImages[p2Total];
        DealBC2.Play("Deal");
        yield return new WaitForSecondsRealtime(2f);
        bankerScore.sprite = bankerTotalImages[b2Total];
        if(drawPlayerThird)
                {
                
                DealPC3.Play("Deal");
                yield return new WaitForSecondsRealtime(2f);
                playerScore.sprite = playerTotalImages[pTotal];
            }
        if (drawBankerThird)
            {                
                DealBC3.Play("Deal");
                yield return new WaitForSecondsRealtime(2f);
                bankerScore.sprite = bankerTotalImages[bTotal];
            }     
       
        yield return new WaitForSecondsRealtime(1f);
        playerScoreW.sprite = playerTotalImages[pTotal];
        bankerScoreW.sprite = playerTotalImages[bTotal];
        nGames++;

        if (winner == 1)
        {
            winnerLogo.sprite = winnerPlayer;
            scores[currentGameNo].sprite = spPlayer;
            scores[currentGameNo].gameObject.SetActive(true);
            currentGameNo++;
            npWins++;
        }
        if (winner == 2)
        {
            winnerLogo.sprite = winnerBanker;
            scores[currentGameNo].sprite = spBanker;
            scores[currentGameNo].gameObject.SetActive(true);
            currentGameNo++;
            nbWins++;
        }
        if (winner == 3)
        {
            winnerLogo.sprite = winnerTie;
            scores[currentGameNo].sprite = spTie;
            scores[currentGameNo].gameObject.SetActive(true);
            currentGameNo++;
            ntWins++;
        }
        if(winner==BetOn)
        {
            resultText.text = winnigText + BetAmount.ToString();
            winnerCrown.gameObject.SetActive(true);
            playerBalace += BetAmount;
            txtPlayerBalance.text = playerBalace.ToString();
        }
        else
        {
            resultText.text = losingText + BetAmount.ToString();
            winnerCrown.gameObject.SetActive(false);
            playerBalace -= BetAmount;
            txtPlayerBalance.text = playerBalace.ToString();
        }
        winnerBoard.gameObject.SetActive(true);
        wbAnimator.Play("ShowWinner");
        wPC1.Play("Flip");
        wPC2.Play("Flip");
        if (drawPlayerThird) wPC3.gameObject.SetActive(true);
        wPC3.Play("Flip");
        wBC1.Play("Flip");
        
        wBC2.Play("Flip");
        if (drawBankerThird) wBC3.gameObject.SetActive(true);
        wBC3.Play("Flip");
        numberOfGames.text = nGames.ToString();
        bankerWins.text = nbWins.ToString();
        playerWins.text = npWins.ToString();
        tieWins.text = ntWins.ToString();
        DealPC1.Play("Shoe");
        DealPC2.Play("Shoe");
        DealPC3.Play("Shoe");
        DealBC1.Play("Shoe");
        DealBC2.Play("Shoe");
        DealBC3.Play("Shoe");
        playerScore.gameObject.SetActive(false);
        bankerScore.gameObject.SetActive(false);
        BetAmount = 0;
        txtBetAmount.text = BetAmount.ToString();
        player.image.sprite = playerDisabled;
        player.enabled = false;
        banker.image.sprite = bankerDisabled;
        banker.enabled = false;
        tie.image.sprite = tieDisabled;
        tie.enabled = false;
        BetOn = 0;
        playerBet.text = "";
        bankerBet.text = "";
        tieBet.text = "";
        yield return new WaitForSecondsRealtime(8f);
        
        k25.enabled = true;
        k50.enabled = true;
        k10.enabled = true;
        k1.enabled = true;
        k5.enabled = true;
        countdownTime = 15;
        txtTimer.text = "15";
        startTimer = true;
        imgTimer.fillAmount = 1;

    }

    IEnumerator MovePlayerThirdCard(float wait)
    {
        yield return new WaitForSecondsRealtime(wait);
        imgPC3.gameObject.SetActive(true);
        while (imgPC3.transform.position != playerCard3TargetPosition)
        {
            yield return new WaitForSecondsRealtime(.01f);
            imgPC3.transform.position = Vector3.MoveTowards(imgPC3.transform.position, playerCard3TargetPosition, speed * 10f);
        }
    }

    IEnumerator MoveBankerThirdCard(float wait)
    {
        yield return new WaitForSecondsRealtime(wait);
        imgBC3.gameObject.SetActive(true);
        while (imgBC3.transform.position != bankerCard3TargetPosition)
        {
            yield return new WaitForSecondsRealtime(.01f);
            imgBC3.transform.position = Vector3.MoveTowards(imgBC3.transform.position, bankerCard3TargetPosition, speed * 10f);
        }
    }
    

    public void ShowCard(int face, Image card, Sprite[] set)
    {
        card.sprite = set[face];
    }

    IEnumerator FlipCard(int face, Image card, Sprite[] set, float wait, bool showTotal = false, bool player = true)
    {

        int ang = 0;
        yield return new WaitForSecondsRealtime(wait);
        for (int i = 0; i < 180; i++)
        {
            yield return new WaitForSecondsRealtime(0.001f);
            card.transform.Rotate(new Vector3(0, 1, 0));

            ang++;
            if (ang == 90 || ang == -90)
            {
                card.sprite = set[face];
            }
        }
        if (showTotal)
        {
            
            yield return new WaitForSecondsRealtime(1);
            if (player)
            {
                if (face > 9) face = 0;
                pTotal += face;
                if (pTotal > 9)
                    pTotal -= 10;
                playerScore.sprite = playerTotalImages[pTotal];
                playerScore.gameObject.SetActive(true);
            }
            else
            {
                if (face > 9) face = 0;
                bTotal += face;
                if (bTotal > 9)
                    bTotal -= 10;
                bankerScore.sprite = bankerTotalImages[bTotal];
                bankerScore.gameObject.SetActive(true);
            }
        }
    }
    IEnumerator Pause(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
    }
    public void PrepareCards()
    {

        winner = 0;
        pTotal = 0;
        bTotal = 0;
        p2Total = 0;
        p1Total = 0;
        b2Total = 0;
        b1Total = 0;
        drawPlayerThird = false;
        drawBankerThird = false;
        CardFace.SetBankerCard1(0);
        CardFace.SetBankerCard2(0);
        CardFace.SetBankerCard3(0);
        CardFace.SetPlayerCard1(0);
        CardFace.SetPlayerCard2(0);
        CardFace.SetPlayerCard3(0);

        int playerC1 = bacCards.GetRandomCard; // get player card 1
        CardFace.SetPlayerCard1(playerC1);
        bacCards.RemoveRandomCard(playerC1);   // remove player card 1     
        int bankerC1 = bacCards.GetRandomCard; // get banker card 1
        CardFace.SetBankerCard1(bankerC1);
        bacCards.RemoveRandomCard(bankerC1);  // remove banker card 1 
        int playerC2 = bacCards.GetRandomCard;
        CardFace.SetPlayerCard2(playerC2);
        bacCards.RemoveRandomCard(playerC2);
        int bankerC2 = bacCards.GetRandomCard;
        CardFace.SetBankerCard2(bankerC2);
        bacCards.RemoveRandomCard(bankerC2);
        pTotal = 0;
        bTotal = 0;
        int playerC3 = 0;
        int bankerC3 = 0;
        playerScore.gameObject.SetActive(false);
        bankerScore.gameObject.SetActive(false);
        int pc1=0, pc2=0,pc3=0, bc1=0, bc2=0,bc3=0;
        // find the exact baccarat value of player cards
        if (playerC1 > 100 && playerC1 < 200)
        {
            pc1 = playerC1 % 100;
            //StartCoroutine(FlipCard(pc1, imgPC1, cardImages1,0,true));
            
        }
        if (playerC1 > 200 && playerC1 < 300)
        {
            pc1 = playerC1 % 200;
            //StartCoroutine(FlipCard(pc1, imgPC1, cardImages2,0, true));
            
        }
        if (playerC1 > 300 && playerC1 < 400)
        {
            pc1 = playerC1 % 300;
            //StartCoroutine(FlipCard(pc1, imgPC1, cardImages3,0, true));
            
        }
        if (playerC1 > 400)
        {
            pc1 = playerC1 % 400;
            //StartCoroutine(FlipCard(pc1, imgPC1, cardImages4,0, true));
            
        }
        if (pc1 > 9)
            pc1 = 0;
        p1Total = pc1;
        pTotal += pc1;

        if (playerC2 > 100 && playerC2 < 200)
        {
            pc2 = playerC2 % 100;
            //StartCoroutine(FlipCard(pc2, imgPC2, cardImages1,1,true));
            
        }
        if (playerC2 > 200 && playerC2 < 300)
        {
            pc2 = playerC2 % 200;
            //StartCoroutine(FlipCard(pc2, imgPC2, cardImages2,1, true));
            
        }
        if (playerC2 > 300 && playerC2 < 400)
        {
            pc2 = playerC2 % 300;
            //StartCoroutine(FlipCard(pc2, imgPC2, cardImages3,1, true));
            
        }
        if (playerC2 > 400)
        {
            pc2 = playerC2 % 400;
            //StartCoroutine(FlipCard(pc2, imgPC2, cardImages4,1, true));
            
        }
        if (pc2 > 9)
            pc2 = 0;

        pTotal += pc2;
        
        // find the total of first two cards on player hands. value greater than 9 must subtract 10 from itself
        //int playerTotal2C = pc1 + pc2;
        if (pTotal > 9)
            pTotal -= 10;
        p2Total = pTotal;
        //pTotal = playerTotal2C;

        // find the exact baccarat value of banker cards
        if (bankerC1 > 100 && bankerC1 < 200)
        {
            bc1 = bankerC1 % 100;
            //StartCoroutine(FlipCard(bc1, imgBC1, cardImages1, 4,true,false));
            
        }
        if (bankerC1 > 200 && bankerC1 < 300)
        {
            bc1 = bankerC1 % 200;
           // StartCoroutine(FlipCard(bc1, imgBC1, cardImages2, 4,true,false));
            
        }
        if (bankerC1 > 300 && bankerC1 < 400)
        {
            bc1 = bankerC1 % 300;
            //StartCoroutine(FlipCard(bc1, imgBC1, cardImages3, 4,true,false));
            
        }
        if (bankerC1 > 400)
        {
            bc1 = bankerC1 % 400;
            //StartCoroutine(FlipCard(bc1, imgBC1, cardImages4, 4, true, false));
            
        }
        if (bc1 > 9)
            bc1 = 0;
        b1Total = bc1;
        bTotal += bc1;

        if (bankerC2 > 100 && bankerC2 < 200)
        {
            bc2 = bankerC2 % 100;
            //StartCoroutine(FlipCard(bc2, imgBC2, cardImages1, 5,true,false));
            
        }
        if (bankerC2 > 200 && bankerC2 < 300)
        {
            bc2 = bankerC2 % 200;
            //StartCoroutine(FlipCard(bc2, imgBC2, cardImages2, 5, true, false));
            
        }
        if (bankerC2 > 300 && bankerC2 < 400)
        {
            bc2 = bankerC2 % 300;
            //StartCoroutine(FlipCard(bc2, imgBC2, cardImages3, 5, true, false));
            
        }
        if (bankerC2 > 400)
        {
            bc2 = bankerC2 % 400;
            //StartCoroutine(FlipCard(bc2, imgBC2, cardImages4, 5, true, false));
            
        }
        if (bc2 > 9)
            bc2 = 0;

        bTotal += bc2;
        // find the total of first two cards on banker hands. value greater than 9 must subtract 10 from itself
        //int bankerTotal2C = bc1 + bc2;
        if (bTotal > 9)
            bTotal -= 10;
        b2Total = bTotal;
        //bTotal = bankerTotal2C;

        //int playerTotal3C = 0;
        //int bankerTotal3C = 0;
        Debug.Log(pTotal);
        Debug.Log(bTotal);
        // decide if any one has a natural win
        // total of 8 or 9 in any hand will make a natural winner
        if (pTotal > 7 && bTotal > 7)
        {
            if (pTotal != bTotal)
            {
                if (pTotal > bTotal)
                    //Debug.Log("player wins with natural " + playerTotal2C);
                    winner = 1;
                else
                    //Debug.Log("banker wins with natural " + bankerTotal2C);
                    winner = 2;
            }
            else
                //Debug.Log("tie with " + bankerTotal2C + " " + playerTotal2C);
                winner = 3;
            return;
        }
        if (pTotal > 7)
        {
            //Debug.Log("player wins with natural " + playerTotal2C);
            winner = 1;
            return;
        }
        if (bTotal > 7)
        {
            //Debug.Log("banker wins with natural " + bankerTotal2C);
            winner = 2;
            return;
        }

        // at this point no one hand has a natural
        // check if the player initial total is 6 or 7
        if(pTotal>5)
        {
            // player must stand
            drawPlayerThird = false;
            // banker now has a chance to draw a third card
            // if banker too is 6 or 7 then it must stand also
            // bigger total value is the winner
            if(bTotal>5)
            {
                drawBankerThird = false;
                if(pTotal!=bTotal)
                {
                    if (pTotal > bTotal)
                        // Debug.Log("Player wins with " + playerTotal2C);
                        winner = 1;
                    else
                        // Debug.Log("Banker wins with " + bankerTotal2C);
                        winner = 2;
                }
                else
                {
                    Debug.Log("Tie with both hands " + pTotal);
                    winner = 3;
                }
                
            }
            else
            {
                // Banker now must draw a third card because it's first two cards value
                // is 5 or less
                //StartCoroutine(MoveBankerThirdCard(8));
                drawBankerThird = true;
                bankerC3 = bacCards.GetRandomCard;
                CardFace.SetBankerCard3(bankerC3);
                bacCards.RemoveRandomCard(bankerC3);
                // now determine the real value of the third card
                // find the exact baccarat value of banker cards
                if (bankerC3 > 100 && bankerC3 < 200)
                {
                    bc3 = bankerC3 % 100;
                    //StartCoroutine(FlipCard(bc3, imgBC3, cardImages1, 9,true,false));
                    
                }
                if (bankerC3 > 200 && bankerC3 < 300)
                {
                    bc3 = bankerC3 % 200;
                   // StartCoroutine(FlipCard(bc3, imgBC3, cardImages2, 9, true, false));
                    
                }
                if (bankerC3 > 300 && bankerC3 < 400)
                {
                    bc3 = bankerC3 % 300;
                    //StartCoroutine(FlipCard(bc3, imgBC3, cardImages3, 9, true, false));
                    
                }
                if (bankerC3 > 400)
                {
                    bc3 = bankerC3 % 400;
                    //StartCoroutine(FlipCard(bc3, imgBC3, cardImages4, 9, true, false));
                    
                }
                if (bc3 > 9)
                    bc3 = 0;
                // now add the third card with first two card total
                 bTotal = bTotal + bc3;
                if (bTotal > 9)
                    bTotal -= 10;
               // bTotal = bankerTotal3C;
                // compare it player total and find the winner
                if (pTotal!=bTotal)
                {
                    if (pTotal > bTotal)
                        // Debug.Log("Player wins with " + playerTotal2C);
                        winner = 1;
                    else
                        //Debug.Log("Banker wins with " + bankerTotal3C);
                        winner = 2;
                }
                else
                {
                    //Debug.Log("Tie with both hands " + playerTotal2C);
                    winner = 3;
                }
            }
            return;
        }

        // at this point, player total must be 5 or less
        // which means player now must draw a third card
        //StartCoroutine(MovePlayerThirdCard(8));
        drawPlayerThird = true;
        playerC3 = bacCards.GetRandomCard;
        CardFace.SetPlayerCard3(playerC3);
        bacCards.RemoveRandomCard(playerC3);
        // now determine the real value of the third card
        // find the exact baccarat value of banker cards
        if (playerC3 > 100 && playerC3 < 200)
        {
            pc3 = playerC3 % 100;
            //StartCoroutine(FlipCard(pc3, imgPC3, cardImages1, 9, true));
            
        }
        if (playerC3 > 200 && playerC3 < 300)
        {
            pc3 = playerC3 % 200;
            //StartCoroutine(FlipCard(pc3, imgPC3, cardImages2, 9, true));
            
        }
        if (playerC3 > 300 && playerC3 < 400)
        {
            pc3 = playerC3 % 300;
            //StartCoroutine(FlipCard(pc3, imgPC3, cardImages3, 9, true));
            
        }
        if (playerC3 > 400)
        {
            pc3 = playerC3 % 400;
            //StartCoroutine(FlipCard(pc3, imgPC3, cardImages4, 9, true));
            
        }
        if (pc3 > 9)
            pc3 = 0;
        // now add the third card with first two card total
         pTotal = pTotal + pc3;
        if (pTotal > 9)
            pTotal -= 10;
       // pTotal = playerTotal3C;
        // now check if the banker's total is 6 or 7
        // no third card for banker if total is 7
        // a third card for banker if total is 6 and player third card is 6 or 7
        if (bTotal==7)
        {
            drawBankerThird = false;
            // compare with player total and determine winner
            if (pTotal != bTotal)
            {
                if (pTotal > bTotal)
                    //Debug.Log("Player wins with " + playerTotal3C);
                    winner = 1;
                else
                    //Debug.Log("Banker wins with " + bankerTotal2C);
                    winner = 2;
            }
            else
            {
                //Debug.Log("Tie with both hands " + playerTotal3C);
                winner = 3;
            }
            return;
        }

        if(bTotal==6)
        {
            // banker now draw a third card if player third card is 6 or 7
            if(pc3==6 || pc3==7)
            {
                drawBankerThird = true;
                //StartCoroutine(MoveBankerThirdCard(10));
                bankerC3 = bacCards.GetRandomCard;
                CardFace.SetBankerCard3(bankerC3);
                bacCards.RemoveRandomCard(bankerC3);
                // now determine the real value of the third card
                // find the exact baccarat value of banker cards
                if (bankerC3 > 100 && bankerC3 < 200)
                {
                    bc3 = bankerC3 % 100;
                    //StartCoroutine(FlipCard(bc3, imgBC3, cardImages1, 11, true,false));
                    
                }
                if (bankerC3 > 200 && bankerC3 < 300)
                {
                    bc3 = bankerC3 % 200;
                    //StartCoroutine(FlipCard(bc3, imgBC3, cardImages2, 11, true, false));
                   
                }
                if (bankerC3 > 300 && bankerC3 < 400)
                {
                    bc3 = bankerC3 % 300;
                    //StartCoroutine(FlipCard(bc3, imgBC3, cardImages3, 11, true, false));
                    
                }
                if (bankerC3 > 400)
                {
                    bc3 = bankerC3 % 400;
                    //StartCoroutine(FlipCard(bc3, imgBC3, cardImages4, 11, true, false));
                    
                }
                if (bc3 > 9)
                    bc3 = 0;
                // now add the third card with first two card total
                bTotal = bTotal + bc3;
                if (bTotal > 9)
                    bTotal -= 10;
                //bTotal = bankerTotal3C;
                // compare it player total and find the winner
                if (pTotal != bTotal)
                {
                    if (pTotal > bTotal)
                        //Debug.Log("Player wins with " + playerTotal3C);
                        winner = 1;
                    else
                        // Debug.Log("Banker wins with " + bankerTotal3C);
                        winner = 2;
                }
                else
                {
                   // Debug.Log("Tie with both hands " + playerTotal3C);
                    winner = 3;
                }
            }
            else
            {
                drawBankerThird = false;
                // banker must stand and we now compare the two hands' total to determine the winner
                if (pTotal != bTotal)
                {
                    if (pTotal > bTotal)
                        //Debug.Log("Player wins with " + playerTotal3C);
                        winner = 1;
                    else
                        //Debug.Log("Banker wins with " + bankerTotal2C);
                        winner = 2;
                }
                else
                {
                    //Debug.Log("Tie with both hands " + playerTotal3C);
                    winner = 3;
                }                
            }
            return;
        }

        // at this point, player has a third card which is 5 or less
        // banker total is neither 6 or 7
        // so banker can draw a third card depending on the value of 
        // player's third card and it's own total value
        // if banker's total is 2 or less, then it can draw a third card no matter what
        if(bTotal<3)
        {
            drawBankerThird = true;
            // StartCoroutine(MoveBankerThirdCard(10));
            bankerC3 = bacCards.GetRandomCard;
            CardFace.SetBankerCard3(bankerC3);
            bacCards.RemoveRandomCard(bankerC3);
            // now determine the real value of the third card
            // find the exact baccarat value of banker cards
            if (bankerC3 > 100 && bankerC3 < 200)
            {
                bc3 = bankerC3 % 100;
                //StartCoroutine(FlipCard(bc3, imgBC3, cardImages1, 11, true, false));
                
            }
            if (bankerC3 > 200 && bankerC3 < 300)
            {
                bc3 = bankerC3 % 200;
                //StartCoroutine(FlipCard(bc3, imgBC3, cardImages2, 11, true, false));
                
            }
            if (bankerC3 > 300 && bankerC3 < 400)
            {
                bc3 = bankerC3 % 300;
                //StartCoroutine(FlipCard(bc3, imgBC3, cardImages3, 11, true, false));
                
            }
            if (bankerC3 > 400)
            {
                bc3 = bankerC3 % 400;
               // StartCoroutine(FlipCard(bc3, imgBC3, cardImages4, 11, true, false));
                
            }
            if (bc3 > 9)
                bc3 = 0;
            // now add the third card with first two card total
            bTotal = bTotal + bc3;
            if (bTotal > 9)
                bTotal -= 10;
            // bTotal = bankerTotal3C;
            // compare it with player total and find the winner
            if (pTotal != bTotal)
            {
                if (pTotal > bTotal)
                    // Debug.Log("Player wins with " + playerTotal3C);
                    winner = 1;
                else
                    //Debug.Log("Banker wins with " + bankerTotal3C);
                    winner = 2;
            }
            else
            {
                //Debug.Log("Tie with both hands " + playerTotal3C);
                winner = 3;
            }
            return;
        }

        // at this point, banker total must be 3, 4 or 5
        // banker will draw a third card depending on the player third card value
        // if banker total is 3, it will draw a third card all the times except when player third card
        // is not 8
        if(bTotal==3)
        {
            if(pc3!=8)
            {
                drawBankerThird = true;
                //StartCoroutine(MoveBankerThirdCard(10));
                bankerC3 = bacCards.GetRandomCard;
                CardFace.SetBankerCard3(bankerC3);
                bacCards.RemoveRandomCard(bankerC3);
                // now determine the real value of the third card
                // find the exact baccarat value of banker cards
                if (bankerC3 > 100 && bankerC3 < 200)
                {
                    bc3 = bankerC3 % 100;
                    //StartCoroutine(FlipCard(bc3, imgBC3, cardImages1, 11, true, false));
                    
                }
                if (bankerC3 > 200 && bankerC3 < 300)
                {
                    bc3 = bankerC3 % 200;
                   // StartCoroutine(FlipCard(bc3, imgBC3, cardImages2, 11, true, false));
                    
                }
                if (bankerC3 > 300 && bankerC3 < 400)
                {
                    bc3 = bankerC3 % 300;
                   // StartCoroutine(FlipCard(bc3, imgBC3, cardImages3, 11, true, false));
                    
                }
                if (bankerC3 > 400)
                {
                    bc3 = bankerC3 % 400;
                    //StartCoroutine(FlipCard(bc3, imgBC3, cardImages4, 11, true, false));
                    
                }
                if (bc3 > 9)
                    bc3 = 0;
                // now add the third card with first two card total
                bTotal = bTotal + bc3;
                if (bTotal > 9)
                    bTotal -= 10;
                //  bTotal = bankerTotal3C;
                // compare it player total and find the winner
                if (pTotal != bTotal)
                {
                    if (pTotal > bTotal)
                        // Debug.Log("Player wins with " + playerTotal3C);
                        winner = 1;
                    else
                        // Debug.Log("Banker wins with " + bankerTotal3C);
                        winner = 2;
                }
                else
                {
                    // Debug.Log("Tie with both hands " + playerTotal3C);
                    winner = 3;
                }
            }
            else
            {
                drawBankerThird = false;
                // banker must stand, because player third card is 8
                // we must compare the two totals to determine the winner
                if (pTotal != bTotal)
                {
                    if (pTotal > bTotal)
                        // Debug.Log("Player wins with " + playerTotal3C);
                        winner = 1;
                    else
                        // Debug.Log("Banker wins with " + bankerTotal2C);
                        winner = 2;
                }
                else
                {
                    // Debug.Log("Tie with both hands " + playerTotal3C);
                    winner = 3;
                }
            }
            return;
        }

        if(bTotal==4)
        {
            // banker can draw a third card, if player third card is 2-7
            if(pc3>1 && pc3<8)
            {
                drawBankerThird = true;
                //StartCoroutine(MoveBankerThirdCard(10));
                bankerC3 = bacCards.GetRandomCard;
                CardFace.SetBankerCard3(bankerC3);
                bacCards.RemoveRandomCard(bankerC3);
                // now determine the real value of the third card
                // find the exact baccarat value of banker cards
                if (bankerC3 > 100 && bankerC3 < 200)
                {
                    bc3 = bankerC3 % 100;
                    //StartCoroutine(FlipCard(bc3, imgBC3, cardImages1, 11, true, false));
                    
                }
                if (bankerC3 > 200 && bankerC3 < 300)
                {
                    bc3 = bankerC3 % 200;
                    StartCoroutine(FlipCard(bc3, imgBC3, cardImages2, 11, true, false));
                    //imgBC3.sprite = cardImages2[bc3];
                }
                if (bankerC3 > 300 && bankerC3 < 400)
                {
                    bc3 = bankerC3 % 300;
                   // StartCoroutine(FlipCard(bc3, imgBC3, cardImages3, 11, true, false));
                    
                }
                if (bankerC3 > 400)
                {
                    bc3 = bankerC3 % 400;
                   // StartCoroutine(FlipCard(bc3, imgBC3, cardImages4, 11, true, false));
                    
                }
                if (bc3 > 9)
                    bc3 = 0;
                // now add the third card with first two card total
                bTotal = bTotal + bc3;
                if (bTotal > 9)
                    bTotal -= 10;
                // bTotal = bankerTotal3C;
                // compare it player total and find the winner
                if (pTotal != bTotal)
                {
                    if (pTotal > bTotal)
                        // Debug.Log("Player wins with " + playerTotal3C);
                        winner = 1;
                    else
                        // Debug.Log("Banker wins with " + bankerTotal3C);
                        winner = 2;
                }
                else
                {
                    //Debug.Log("Tie with both hands " + playerTotal3C);
                    winner = 3;
                }
            }
            else
            {
                drawBankerThird = false;
                // banker must stand because player's third card is out of range
                // we must compare the two totals to determine the winner
                if (pTotal != bTotal)
                {
                    if (pTotal > bTotal)
                        // Debug.Log("Player wins with " + playerTotal3C);
                        winner = 1;
                    else
                        // Debug.Log("Banker wins with " + bankerTotal2C);
                        winner = 2;
                }
                else
                {
                    //  Debug.Log("Tie with both hands " + playerTotal3C);
                    winner = 3;
                }
            }
            return;
        }
        // the last situation left
        if (bTotal == 5)
        {
            // banker can draw a third card, if player third card is 4-7
            if (pc3 > 3 && pc3 < 8)
            {
                drawBankerThird = true;
                // StartCoroutine(MoveBankerThirdCard(10));
                bankerC3 = bacCards.GetRandomCard;
                CardFace.SetBankerCard3(bankerC3);
                bacCards.RemoveRandomCard(bankerC3);
                // now determine the real value of the third card
                // find the exact baccarat value of banker cards
                if (bankerC3 > 100 && bankerC3 < 200)
                {
                    bc3 = bankerC3 % 100;
                    //StartCoroutine(FlipCard(bc3, imgBC3, cardImages1, 11, true, false));
                    
                }
                if (bankerC3 > 200 && bankerC3 < 300)
                {
                    bc3 = bankerC3 % 200;
                   // StartCoroutine(FlipCard(bc3, imgBC3, cardImages2, 11, true, false));
                   
                }
                if (bankerC3 > 300 && bankerC3 < 400)
                {
                    bc3 = bankerC3 % 300;
                   // StartCoroutine(FlipCard(bc3, imgBC3, cardImages3, 11, true, false));
                    
                }
                if (bankerC3 > 400)
                {
                    bc3 = bankerC3 % 400;
                   // StartCoroutine(FlipCard(bc3, imgBC3, cardImages4, 11, true, false));
                    
                }
                if (bc3 > 9)
                    bc3 = 0;
                // now add the third card with first two card total
                bTotal = bTotal + bc3;
                if (bTotal > 9)
                    bTotal -= 10;
                // bTotal = bankerTotal3C;
                // compare it player total and find the winner
                if (pTotal != bTotal)
                {
                    if (pTotal > bTotal)
                        // Debug.Log("Player wins with " + playerTotal3C);
                        winner = 1;
                    else
                        //Debug.Log("Banker wins with " + bankerTotal3C);
                        winner = 2;
                }
                else
                {
                    //Debug.Log("Tie with both hands " + playerTotal3C);
                    winner = 3;
                }
            }
            else
            {
                drawBankerThird = false;
                // banker must stand because player third card is out of range
                // we must compare the two totals to determine the winner
                if (pTotal != bTotal)
                {
                    if (pTotal > bTotal)
                        //Debug.Log("Player wins with " + playerTotal3C);
                        winner = 1;
                    else
                        // Debug.Log("Banker wins with " + bankerTotal2C);
                        winner = 2;
                }
                else
                {
                    // Debug.Log("Tie with both hands " + playerTotal3C);
                    winner = 3;
                }
            }
        }
        // our baccarat winner is determined finally above
    }
}
