using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class GirlTTPrefabMaker : MonoBehaviour
{    
    public bool allOptions;
    int hair;
    int chest;
    int legs;
    int feet;
    int tie;
    int jacket;
    int skintone;
    public bool legsactive;
    public bool hoodactive;
    public bool hoodon;
    public bool hoodup;
    public bool glassesactive;
    public bool jacketactive;
    public bool hatactive;
    GameObject GOhead;
    GameObject[] GOfeet;
    GameObject[] GOhair;
    GameObject[] GOchest;
    GameObject[] GOlegs;
    GameObject GOglasses;
    GameObject GOjacket;
    GameObject[] GOhoods;
    Object[] MATskins;
    Object[] MAThairs;
    Object[] MAThairA;
    Object[] MAThairB;
    Object[] MAThairC;
    Object[] MAThairD;
    Object[] MAThairE;
    Object[] MAThairF;
    Object[] MAThairG;
    Object[] MATeyes;
    Object[] MATglasses;
    Object[] MATtshirt;    
    Object[] MATjacket;
    Object[] MATsweater;
    Object[] MATlegs;
    Object[] MATfeetA;
    Object[] MATfeetB;
    Object[] MATfeetC;
    Object[] MAThatA;
    Object[] MAThatB;
    Object[] MAThoods;    
    int eyeindex;
    int skinindex;
    string model;

    void Start()
    {
        allOptions = false;
    }

    public void Getready()
    {
        GOhead = (GetComponent<Transform>().GetChild(0).gameObject);
        Identifymodel();

        GetComponent<Transform>().GetChild(1).gameObject.SetActive(false); 
        GOfeet = new GameObject[5] ;
        GOhair = new GameObject[7] ;
        GOchest = new GameObject[12] ;
        GOlegs = new GameObject[11] ;
        GOhoods = new GameObject[2];
        MAThairs = new Object[19];
        MAThairA = new Object[4];
        MAThairB = new Object[4];
        MAThairC = new Object[4];
        MAThairD = new Object[4];
        MAThairE = new Object[4];
        MAThairF = new Object[4];
        MAThairG = new Object[4];

        //load models
        GOjacket = (GetComponent<Transform>().GetChild(21).gameObject);
        for (int forAUX = 0; forAUX < 5; forAUX++) GOfeet[forAUX] = (GetComponent<Transform>().GetChild(forAUX+7).gameObject);
        for (int forAUX = 0; forAUX < 7; forAUX++) GOhair[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 12).gameObject);
        for (int forAUX = 0; forAUX < 4; forAUX++) GOchest[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 3).gameObject);
        for (int forAUX = 0; forAUX < 8; forAUX++) GOchest[forAUX+4] = (GetComponent<Transform>().GetChild(forAUX + 33).gameObject);
        for (int forAUX = 0; forAUX < 11; forAUX++) GOlegs[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 22).gameObject);
        for (int forAUX = 0; forAUX < 2; forAUX++) GOhoods[forAUX] = (GetComponent<Transform>().GetChild(forAUX + 19).gameObject);        
        GOglasses = transform.Find("ROOT/TT/TT Pelvis/TT Spine/TT Spine1/TT Spine2/TT Neck/TT Head/Glasses").gameObject as GameObject;

        //load  materials
        MATskins = Resources.LoadAll("materials/GIRL/skin/" + model , typeof(Material));
        MAThairs = Resources.LoadAll("materials/GIRL/hairs", typeof (Material));
        MATglasses = Resources.LoadAll("materials/COMMON/glasses", typeof(Material));
        MATeyes = Resources.LoadAll("materials/COMMON/eyes", typeof(Material));
        MATtshirt = Resources.LoadAll("materials/GIRL/tshirt", typeof(Material));        
        MATjacket = Resources.LoadAll("materials/GIRL/jacket", typeof(Material));
        MATsweater = Resources.LoadAll("materials/COMMON/sweaters", typeof(Material));
        MATlegs = Resources.LoadAll("materials/COMMON/legs", typeof(Material));
        MATfeetA = Resources.LoadAll("materials/COMMON/feetA", typeof(Material));
        MATfeetB = Resources.LoadAll("materials/COMMON/feetB", typeof(Material));
        MATfeetC = Resources.LoadAll("materials/COMMON/feetC", typeof(Material));
        MAThatA = Resources.LoadAll("materials/COMMON/hatsA", typeof(Material));
        MAThatB = Resources.LoadAll("materials/COMMON/hatsB", typeof(Material));
        MAThoods = Resources.LoadAll("materials/COMMON/hoods", typeof(Material));

        for (int forAUX = 0; forAUX < 4; forAUX++)
        {
            MAThairA[forAUX] = MAThairs[forAUX];
            MAThairB[forAUX] = MAThairs[forAUX + 4];
            MAThairC[forAUX] = MAThairs[forAUX + 8];
            MAThairD[forAUX] = MAThairs[forAUX + 12];
            MAThairE[forAUX] = MAThairs[forAUX + 16];
            MAThairF[forAUX] = Resources.Load("materials/COMMON/hair and teeth/TTHairF", typeof(Material));
            MAThairG[forAUX] = Resources.Load("materials/GIRL/hairs/TTFHairG0" + (forAUX), typeof(Material));
        }

        if (GOfeet[0].activeSelf && GOfeet[1].activeSelf && GOfeet[2].activeSelf)
        {
            Resetskin(MATskins[0] as Material);
            Randomize();
        }
        else
        {
            for (int forAUX = 0; forAUX < GOhair.Length ; forAUX++) { if (GOhair[forAUX].activeSelf) hair = forAUX; }
            while (!GOchest[chest].activeSelf) chest++;
            if (chest == 0 || chest > 3) while (!GOlegs[legs].activeSelf) legs++;
            while (!GOfeet[feet].activeSelf) feet++;
            if (GOjacket.activeSelf) jacketactive = true; 
            Checklegs();
            if (GOchest[4].activeSelf) hoodactive = true;
            if (GOhoods[0].activeSelf) { hoodon = true; hoodup = false; }
            if (GOhoods[1].activeSelf) { hoodon = true; hoodup = true;hair = 0; }
            if (!GOhoods[0].activeSelf && !GOhoods[1].activeSelf) hoodon = false;
            if (hair > 4) hatactive = true;

            Material[] AUXmaterials; int MATindex = 0;
            AUXmaterials = GOhead.GetComponent<Renderer>().sharedMaterials;
            while (AUXmaterials[skinindex].name != MATskins[MATindex].name) MATindex++;
            Resetskin(MATskins[MATindex] as Material);
        }
    }


    public void Identifymodel()
    {
        Object[] tempMATA = Resources.LoadAll("materials/GIRL/skin/TTGirlA", typeof(Material));
        Object[] tempMATB = Resources.LoadAll("materials/GIRL/skin/TTGirlB", typeof(Material));
        Object[] tempMATC = Resources.LoadAll("materials/GIRL/skin/TTGirlC", typeof(Material));
        Object[] tempMATD = Resources.LoadAll("materials/GIRL/skin/TTGirlD", typeof(Material));
        string theskin = GOhead.GetComponent<Renderer>().sharedMaterials[1].name;
        for (int forAUX = 0; forAUX < tempMATA.Length; forAUX++)
        {
            if (theskin == tempMATA[forAUX].name) model = "TTGirlA";
        }

        theskin = GOhead.GetComponent<Renderer>().sharedMaterials[0].name;

        for (int forAUX = 0; forAUX < tempMATA.Length; forAUX++)
        {
            if (theskin == tempMATA[forAUX].name) model = "TTGirlA";
        }
        for (int forAUX = 0; forAUX < tempMATB.Length; forAUX++)
        {
            if (theskin == tempMATB[forAUX].name) model = "TTGirlB";
        }
        for (int forAUX = 0; forAUX < tempMATC.Length; forAUX++)
        {
            if (theskin == tempMATC[forAUX].name) model = "TTGirlC";
        }
        for (int forAUX = 0; forAUX < tempMATD.Length; forAUX++)
        {
            if (theskin == tempMATD[forAUX].name) model = "TTGirlD";
        }
        if (model == "TTGirlA") { eyeindex = 0; skinindex = 1; }
        if (model == "TTGirlB") { eyeindex = 1; skinindex = 0; }
        if (model == "TTGirlC") { eyeindex = 2; skinindex = 0; }
        if (model == "TTGirlD") { eyeindex = 3; skinindex = 0; }
    }
    public void Deactivateall()
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) GOhair[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) GOlegs[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) GOfeet[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOhoods.Length; forAUX++) GOhoods[forAUX].SetActive(false);
        GOjacket.SetActive(false);
        GOglasses.SetActive(false);
        jacketactive = false;
        glassesactive = false;
        hoodactive = false;
    }
    public void Activateall()
    {
        for (int forAUX = 0; forAUX < GOhair.Length; forAUX++) GOhair[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) GOlegs[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) GOfeet[forAUX].SetActive(true);
        for (int forAUX = 0; forAUX < GOhoods.Length; forAUX++) GOhoods[forAUX].SetActive(true);
        GOjacket.SetActive(true);
        GOglasses.SetActive(true);
        jacketactive = true;
        glassesactive = true;
        hoodactive = true;
    }
    public void Menu()
    {
        allOptions = !allOptions;
    }
    
    public void Checklegs()
    {
        if (chest >0 && chest <4)
        {
            legsactive = false;
            GOlegs[legs].SetActive(false);
        }
        else
        {
            legsactive = true;
            GOlegs[legs].SetActive(true);
        }
    }
    public void Checkhood()
    {
        if (chest ==4 )
        {
            hoodactive = true;
            if(hoodon) GOhoods[0].SetActive(true);
            GOhoods[1].SetActive(false);
        }
        else
        {
            hoodactive = false;
            GOhoods[0].SetActive(false);
            GOhoods[1].SetActive(false);
        }
    }
    public void Hoodonoff()
    {
        hoodon = !hoodon;
        GOhoods[0].SetActive(hoodon);
        GOhoods[1].SetActive(false);
        if (!hoodon)  GOhair[hair].SetActive(true); 
    }
    public void Hoodupdown()
    {
        if (GOhoods[0].activeSelf)
        {
            GOhoods[0].SetActive(false);
            GOhoods[1].SetActive(true);
            hoodup = true;
            GOhair[hair].SetActive(false);
            hatactive = false;
        }
        else
        {
            GOhoods[1].SetActive(false);
            GOhoods[0].SetActive(true);
            hoodup = false;
            GOhair[hair].SetActive(true);
            if (hair>4) hatactive = true;
        }
    }
    public void Glasseson()
    {
        glassesactive = !glassesactive;
        GOglasses.SetActive(glassesactive);
    }
    public void Jacketon()
    {
        jacketactive = !jacketactive;
        GOjacket.SetActive(jacketactive);
    }
    
    //models
    public void Nexthat()
    {
        if(hoodactive && hoodup)
        {
            Hoodupdown();
        }
        if (hair < 5)
        {
                GOhair[hair].SetActive(false);
                hair = 5;
                GOhair[hair].SetActive(true);
                hatactive = true;
        }
        else
        {
                GOhair[hair].SetActive(false);
                if (hair < GOhair.Length - 1)
                {
                    hair++;                    
                }
                else
                {
                    hair = 5;
                }
                hatactive = true;
                GOhair[hair].SetActive(true);
        }
        
    }
    public void Prevhat()
    {
        if (hoodactive && hoodup)
        {
            Hoodupdown();
        }
        if (hair < 6)
        {
            GOhair[hair].SetActive(false);
            hair = 6;
            GOhair[hair].SetActive(true);
            hatactive = true;
        }
        else
        {
            GOhair[hair].SetActive(false);
            if (hair > 5)
            {
                hair--;
                hatactive = true;
            }
            else
            {
                hair = 3;
                hatactive = false;
            }
            GOhair[hair].SetActive(true);
        }
    }
    public void Nexthair()
    {
        if (hoodup) Hoodupdown();
        GOhair[hair].SetActive(false);
        if (hatactive) hair = 0;
        hatactive = false;
        if (hair < GOhair.Length-3) hair++;
        else hair = 0;
        GOhair[hair].SetActive(true);        
    }
    public void Prevhair()
    {
        if (hoodup) Hoodupdown();
        GOhair[hair].SetActive(false);
        if (hatactive)hair= GOhair.Length - 3; 
        hatactive = false;
        if (hair > 0) hair--;
        else hair = GOhair.Length-3;
        GOhair[hair].SetActive(true);
    }
    public void Nextchest()
    {
        GOchest[chest].SetActive(false);
        if (chest < GOchest.Length - 1) chest++;
        else chest = 0;
        GOchest[chest].SetActive(true);
        Checkhood();
        Checklegs();
    }
    public void Prevchest()
    {
        GOchest[chest].SetActive(false);
        chest--;
        if (chest < 0) chest = GOchest.Length - 1;        
        GOchest[chest].SetActive(true);
        Checkhood();
        Checklegs();
    }
    public void Nextlegs()
    {
        GOlegs[legs].SetActive(false);
        if (legs < GOlegs.Length - 1) legs++;
        else legs = 0;
        GOlegs[legs].SetActive(true);
    }
    public void Prevlegs()
    {
        GOlegs[legs].SetActive(false);
        if (legs > 0)  legs--;
        else legs = GOlegs.Length - 1;
        GOlegs[legs].SetActive(true);
    }
    public void Nextfeet()
    {
        GOfeet[feet].SetActive(false);
        if (feet < GOfeet.Length - 1) feet++;
        else feet = 0;
        GOfeet[feet].SetActive(true);
    }    
    public void Prevfeet()
    {
        GOfeet[feet].SetActive(false);
        if (feet > 0) feet--;
        else feet = GOfeet.Length - 1;
        GOfeet[feet].SetActive(true);
    }
    public void Nude()
    {
        for (int forAUX = 0; forAUX < GOchest.Length; forAUX++) GOchest[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOlegs.Length; forAUX++) GOlegs[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOfeet.Length; forAUX++) GOfeet[forAUX].SetActive(false);
        for (int forAUX = 0; forAUX < GOhoods.Length; forAUX++) GOhoods[forAUX].SetActive(false);
        GOjacket.SetActive(false);
        jacketactive = false;
        hoodactive = false;
        GOchest[0].SetActive(true);
        GOlegs[0].SetActive(true);
        GOfeet[0].SetActive(true);
    }

    //materials
    public void Nextskincolor(int todo)
    {
        //head
        SetGOmaterials(GOhead, MATskins, skinindex, 0);
        //chest
        SetGOmaterial(GOchest[0], MATskins, 0);
        for (int forAUX = 1; forAUX < 12; forAUX++)
        {
            SetGOmaterials(GOchest[forAUX], MATskins, 0, 0);
        }
        //legs
        SetGOmaterial(GOlegs[0], MATskins, 0);
        SetGOmaterials(GOlegs[7], MATskins, 1,0);
        for (int forAUX = 8; forAUX < 11; forAUX++)
        {
            SetGOmaterials(GOlegs[forAUX], MATskins, 0, 0);            
        }
        //feet
        SetGOmaterial(GOfeet[0], MATskins, 0);
        for (int forAUX = 1; forAUX < 5; forAUX++)
        {
            SetGOmaterials(GOfeet[forAUX], MATskins, 1, 0);
        }
    }
    public void Nextglasses(int todo)
    {
        SetGOmaterial(GOglasses, MATglasses, 0);        
    }
    public void Nexteyescolor(int todo)
    {
        SetGOmaterials(GOhead, MATeyes, eyeindex, 0);            
    }
    public void Nexthaircolor(int todo)
    {

        if (hair == 0) SetGOmaterial(GOhair[0], MAThairA, 0);
        if (hair == 1) SetGOmaterial(GOhair[1], MAThairB, 0);
        if (hair == 2) SetGOmaterial(GOhair[2], MAThairC, 0);
        if (hair == 3) SetGOmaterial(GOhair[3], MAThairD, 0);
        if (hair == 4) SetGOmaterial(GOhair[4], MAThairE, 0);        
        if (hair == 5) SetGOmaterials(GOhair[5], MAThairE, 1, 0);
        if (hair == 6) SetGOmaterials(GOhair[6], MAThairD, 1, 0);
    }
    public void Nexthatcolor(int todo)
    {
        if (hatactive && !hoodup)
        {
            if (hair == 5) Setmaterials(GOhair, MAThatA, 0, 0);
            if (hair == 6) Setmaterials(GOhair, MAThatB, 0, 0);
        }
    }
    public void Nextchestcolor(int todo)
    {
        if (chest >0 && chest < 4) Setmaterials(GOchest, MATtshirt, 1, 0);
        if (chest == 4)
        {
            Setmaterials(GOchest, MATsweater, 1, 0);
            SetGOmaterial(GOhoods[0], MATsweater, 0);
            SetGOmaterial(GOhoods[1], MAThoods, 0);
        }
        if (chest > 4 ) Setmaterials(GOchest, MATtshirt, 1, 0);        
    }
    public void Nextjacketcolor(int todo)
    {
        SetGOmaterial(GOjacket, MATjacket, 0);
    }
    public void Nextlegscolor(int todo)
    {
        if (legsactive)
        {
            if (legs > 0 && legs < 7) Setmaterial(GOlegs, MATlegs, 0);
            if (legs == 7) Setmaterials(GOlegs, MATlegs, 0, 0);
            if (legs == 8 || legs == 9) Setmaterials(GOlegs, MATlegs, 1, 0);
            if (legs == 10) Setmaterials(GOlegs, MATtshirt, 1, 0);
        }
    }
    public void Nextfeetcolor(int todo)
    {
        if (feet == 1) Setmaterials(GOfeet, MATfeetA,0, 0);
        if (feet == 2) Setmaterials(GOfeet, MATfeetB,0, 0);
        if (feet == 3||feet==4) Setmaterials(GOfeet, MATfeetC, 0, 0);
    }
   
    
    public void Resetmodel()
    {
        Resetskin(MATskins[0] as Material);
        Activateall();
        Menu();
    }
    public void Resetskin(Material skinbase)
    {
        //head
        Material[] AUXmaterials;
        AUXmaterials = GOhead.GetComponent<Renderer>().sharedMaterials;
        AUXmaterials[skinindex] = skinbase;
        GOhead.GetComponent<Renderer>().sharedMaterials = AUXmaterials;
        //chest  
        GOchest[0].GetComponent<Renderer>().sharedMaterial = skinbase;
        for (int forAUX = 1; forAUX < GOchest.Length; forAUX++)
        {
            AUXmaterials = GOchest[forAUX].GetComponent<Renderer>().sharedMaterials;
            AUXmaterials[0] = skinbase;
            GOchest[forAUX].GetComponent<Renderer>().sharedMaterials = AUXmaterials;
        }
        //legs
        GOlegs[0].GetComponent<Renderer>().sharedMaterial = skinbase;
        AUXmaterials = GOlegs[7].GetComponent<Renderer>().sharedMaterials;
        AUXmaterials[1] = skinbase;
        GOlegs[7].GetComponent<Renderer>().sharedMaterials = AUXmaterials;
        for (int forAUX = 8; forAUX < GOlegs.Length; forAUX++)
        {
            AUXmaterials = GOlegs[forAUX].GetComponent<Renderer>().sharedMaterials;
            AUXmaterials[0] = skinbase;
            GOlegs[forAUX].GetComponent<Renderer>().sharedMaterials = AUXmaterials;            
        }
        //feet
        GOfeet[0].GetComponent<Renderer>().sharedMaterial = skinbase;
        for (int forAUX = 1; forAUX < GOfeet.Length; forAUX++)
        {
            AUXmaterials = GOfeet[forAUX].GetComponent<Renderer>().sharedMaterials;
            AUXmaterials[1] = skinbase;
            GOfeet[forAUX].GetComponent<Renderer>().sharedMaterials = AUXmaterials;            
        }
    }
    public void Randomize()
    {
        Deactivateall();
        //models
        hair = Random.Range(0,7); 
        GOhair[hair].SetActive(true);
        if (hair > 4) hatactive = true; else hatactive = false;
        chest = Random.Range(1, GOchest.Length); GOchest[chest].SetActive(true);
        legs = Random.Range(1, GOlegs.Length); GOlegs[legs].SetActive(true);
        feet = Random.Range(1, GOfeet.Length); GOfeet[feet].SetActive(true);

        if (Random.Range(0, 4) > 2) jacketactive = true; else jacketactive = false;
        GOjacket.SetActive(jacketactive);

        if (Random.Range(0, 4) > 2)
        {
            glassesactive = true;
            GOglasses.SetActive(true);
            SetGOmaterial(GOglasses, MATglasses, 2);
        }
        else glassesactive = false;

        Checklegs();

        //hood
        Checkhood();
        if (hoodactive)
        {
            if (Random.Range(0, 5) > 2)
            {
                hoodon=true;
                if (Random.Range(0, 5) > 2) Hoodupdown();
            }
        }
        //materials
        SetGOmaterials(GOhead, MATeyes,eyeindex, 2);        
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 4)); forAUX2++) Nextskincolor(0);        
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 4)); forAUX2++) Nexthaircolor(0);        
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 13)); forAUX2++) Nextfeetcolor(0);
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 25)); forAUX2++) Nextlegscolor(0);        
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 17)); forAUX2++) Nextchestcolor(0);
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 13)); forAUX2++) Nextjacketcolor(0);
        for (int forAUX2 = 0; forAUX2 < (Random.Range(0, 12)); forAUX2++) Nexthatcolor(0);   
    }
    public void CreateCopy()
    {
        GameObject newcharacter = Instantiate(gameObject, transform.position, transform.rotation);
        for (int forAUX = 40; forAUX > 0; forAUX--)
        {
            if (!newcharacter.transform.GetChild(forAUX).gameObject.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(forAUX).gameObject);            
        }
        if (!GOglasses.activeSelf) DestroyImmediate(newcharacter.transform.Find("ROOT/TT/TT Pelvis/TT Spine/TT Spine1/TT Spine2/TT Neck/TT Head/Glasses").gameObject as GameObject);
        DestroyImmediate(newcharacter.GetComponent<GirlTTPrefabMaker>());
    }
    public void FIX()
    {
        GameObject newcharacter = Instantiate(gameObject, transform.position, transform.rotation);
        for (int forAUX = 40; forAUX > 0; forAUX--)
        {
            if (!newcharacter.transform.GetChild(forAUX).gameObject.activeSelf) DestroyImmediate(newcharacter.transform.GetChild(forAUX).gameObject);
        }
        if (!GOglasses.activeSelf) DestroyImmediate(newcharacter.transform.Find("ROOT/TT/TT Pelvis/TT Spine/TT Spine1/TT Spine2/TT Neck/TT Head/Glasses").gameObject as GameObject);
        DestroyImmediate(newcharacter.GetComponent<GirlTTPrefabMaker>());
        DestroyImmediate(gameObject);
    }


    public void Setmaterial(GameObject[] GO, Object[] MAT, int todo)
    {
        int GOindex = 0;
        int MATindex = 0;
        Material AUXmaterial;
        for (int forAUX = 0; forAUX < GO.Length; forAUX++)
        {
            if (GO[forAUX].activeSelf) GOindex = forAUX;
        }
        AUXmaterial = GO[GOindex].GetComponent<Renderer>().sharedMaterial;
        while (AUXmaterial.name != MAT[MATindex].name) MATindex++;

        if (todo==0) //increase
        {
            MATindex++;
            if (MATindex > MAT.Length-1) MATindex = 0;
        }
        if (todo==1) //decrease
        {
            MATindex--;
            if (MATindex < 0) MATindex = MAT.Length-1;
        }
        if (todo == 2) //random value
        {
            MATindex = Random.Range(0, MAT.Length);            
        }
        AUXmaterial = MAT[MATindex] as Material;
        GO[GOindex].GetComponent<Renderer>().sharedMaterial = AUXmaterial;
    }
    public void Setmaterials(GameObject[] GO, Object[] MAT, int matchannel, int todo)
    {
        int GOindex = 0;
        int MATindex = 0;
        Material[] AUXmaterials;
        for (int forAUX = 0; forAUX < GO.Length; forAUX++)
        {
            if (GO[forAUX].activeSelf) GOindex = forAUX;
        }
        AUXmaterials = GO[GOindex].GetComponent<Renderer>().sharedMaterials;
        while (AUXmaterials[matchannel].name != MAT[MATindex].name)
        {
            MATindex++;
        }
        if (todo == 0) //increase
        {
            MATindex++;
            if (MATindex > MAT.Length - 1) MATindex = 0;
        }
        if (todo == 1) //decrease
        {
            MATindex--;
            if (MATindex < 0) MATindex = MAT.Length - 1;
        }
        if (todo == 2) //random value
        {
            MATindex = Random.Range(0, MAT.Length);
        }
        AUXmaterials[matchannel] = MAT[MATindex] as Material; 
        GO[GOindex].GetComponent<Renderer>().sharedMaterials = AUXmaterials;
    }
    public void SetGOmaterial(GameObject GO, Object[] MAT, int todo)
    {
        int MATindex = 0;
        Material AUXmaterial;        
        AUXmaterial = GO.GetComponent<Renderer>().sharedMaterial;
        while (AUXmaterial.name != MAT[MATindex].name) MATindex++;
        if (todo == 0) //increase
        {
            MATindex++;
            if (MATindex > MAT.Length - 1) MATindex = 0;
        }
        if (todo == 1) //decrease
        {
            MATindex--;
            if (MATindex < 0) MATindex = MAT.Length - 1;
        }
        if (todo == 2) //random value
        {
            MATindex = Random.Range(0, MAT.Length);
        }
        AUXmaterial = MAT[MATindex] as Material;        
        GO.GetComponent<Renderer>().sharedMaterial = AUXmaterial;
    }
    public void SetGOmaterials(GameObject GO, Object[] MAT, int matchannel, int todo)
    {
        int MATindex = 0;
        Material[] AUXmaterials;        
        AUXmaterials = GO.GetComponent<Renderer>().sharedMaterials;
        while (AUXmaterials[matchannel].name != MAT[MATindex].name) MATindex++;
        if (todo == 0) //increase
        {
            MATindex++;
            if (MATindex > MAT.Length - 1) MATindex = 0;
        }
        if (todo == 1) //decrease
        {
            MATindex--;
            if (MATindex < 0) MATindex = MAT.Length - 1;
        }
        if (todo == 2) //random value
        {
            MATindex = Random.Range(0, MAT.Length);
        }
        AUXmaterials[matchannel] = MAT[MATindex] as Material;
        GO.GetComponent<Renderer>().sharedMaterials = AUXmaterials;
    }

}