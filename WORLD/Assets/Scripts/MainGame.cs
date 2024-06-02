using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public enum Block_Type
{
    Rock, Earth, Grass, Bronze, Iron, Masonry, Water
}
public struct FaceInformation
{
    public Vector3 v3;
    public string Other;
    public FaceInformation(Vector3 v, string s)
    {
        v3 = v;
        Other = s;
    }
}
public class MainGame : MonoBehaviour
{ 
    public Transform ����;
    public static bool isGameTime;
    public static MainGame mainGame;
    public static UnitObjBase[,,] unit;
    public static List<int> MapMsg;
    public List<UnitObjBase> prefab;
    public static Dictionary<string, FaceInformation> Direction = new Dictionary<string, FaceInformation>();
    public static int World_x = 15;
    public static int World_y = 8;
    public static int World_z = 15;
    public PlayerText player;

    public UnitObjBase target;
    public BagManager bag;
    public Load��� loadPanel;

    public void ExitGameToRoom()
    {
        ExitGame msg = new ExitGame();
        NetManager.Send(msg);
    }
    void Awake()
    {
        isGameTime = false;
        mainGame = GameObject.Find("MainGame").GetComponent<MainGame>();
        InitDirection();
    }
    private void InitDirection()
    {
        Direction.Add("Up", new FaceInformation(new Vector3(0, 1, 0), "Down"));
        Direction.Add("Down", new FaceInformation(new Vector3(0, -1, 0), "Up"));
        Direction.Add("Front", new FaceInformation(new Vector3(0, 0, 1), "Behind"));
        Direction.Add("Behind", new FaceInformation(new Vector3(0, 0, -1), "Front"));
        Direction.Add("Left", new FaceInformation(new Vector3(-1, 0, 0), "Right"));
        Direction.Add("Right", new FaceInformation(new Vector3(1, 0, 0), "Left"));
    }


    //�����ϲ��ʯͷBlock�ϴ���������
    public void CreateEarth()
    {
        for (int i = 1; i < World_x - 1; i++)
        {
            for (int k = 1; k < World_z - 1; k++)
            {
                int topBlock = 0;
                for (int j = 1; j < 2 * World_y - 1; j++)
                {
                    if (unit[i, j + 1, k].unitType == UnitType.Null && unit[i, j, k].unitType == UnitType.Block)
                    {
                        topBlock = j + 1;
                    }
                }
                if (topBlock == 0)
                {
                    topBlock = 1;
                }
                for (int l = 0; l < 4; l++)
                {
                    ChangeObjUnit(i, topBlock + l, k, 2);
                }
            }
        }
        Debug.Log("���� �������");
    }
    private void CreatingTerrain()
    {
        //��������ص㣬�����С��
        int changeNum = UnityEngine.Random.Range(2, Math.Max(2, World_x / 3));
        bool succ;
        {
            //����ɽ����¼�ص�
            for (int i = 0; i < changeNum; i++)
            {
                succ = CreateMountainOnTopBlock();
                //���ʧ�ܣ���˴β���
                if (!succ)
                {
                    changeNum--;
                }

            }
        }
        Debug.Log("ɽ �������");
        changeNum = UnityEngine.Random.Range(4, Math.Max(4, World_x / 2));
        succ = false;
        {
            //�����ݵأ���¼�ص�
            for (int i = 0; i < changeNum; i++)
            {
                succ = CreatePicOnTopBlock();
                //���ʧ�ܣ���˴β���
                if (!succ)
                {
                    changeNum--;
                }
            }
        }
        Debug.Log("�ݵ� �������");
    }
    private bool CreateMountainOnTopBlock()
    {

        //���������
        int _x = UnityEngine.Random.Range(1, World_x - 1);
        int _z = UnityEngine.Random.Range(1, World_z - 1);
        int j = World_y - 1;
        if (unit[_x, j, _z].unitType == UnitType.Block)
        {
            return false;
        }
        //����һ��Բ��
        int h = 0;//ɽ�Ĳ���
        int hTop = UnityEngine.Random.Range(4, 10);//ɽ�Ĳ���
        int r = UnityEngine.Random.Range(5, 10);
        while (h < hTop)
        {
            r -= h;//ɽ�İ뾶
            if (r == 0)
            {
                break;
            }
            //ͨ��x���y��ֵ
            int y1, y2;
            for (int i = -r; i <= r; i++)
            {
                ReturnY(i, r, out y1, out y2);
                //����y��ÿһ�н���ʵ����
                for (int k = y2; k <= y1; k++)
                {
                    //����ǵ�ͼ��ʱ,��һ�������࿪ʼ�������������ϴ�����������Ҫ�ж��ĸ������Ƿ�Խ��
                    if (_x + i >= World_x - 1 || _z + k >= World_z - 1 || _x + i < 1 || _z + k < 1 || j + h >= 2 * World_y - 1)
                    {
                        continue;
                    }
                    ChangeObjUnit(_x + i, j + h, _z + k, 1);
                }
            }
            h++;
        }
        return true;

    }
    private void ReturnY(int x, int r, out int y1, out int y2)
    {
        double x_2 = x * x;
        y1 = (int)Math.Round(Math.Pow(r * r - x_2, 0.5f));
        y2 = -y1;
    }
    /// <summary>
    /// ����һ�������������һ�㿪ʼ����������ʧ�ܻ��ط��ؽ�
    /// </summary>
    public bool CreatePicOnTopBlock()
    {
        //���������
        int _x = UnityEngine.Random.Range(1, World_x - 1);
        int _z = UnityEngine.Random.Range(1, World_z - 1);
        //�������һ����
        for (int j = 1; j < 2 * World_y - 1; j++)
        {
            if (unit[_x, j + 1, _z] == null)
            {
                return false;
            }
            //����һ��Ϊ��ʱ
            if (unit[_x, j + 1, _z].unitType == UnitType.Null)
            {
                //ɾ��һ�����������ķ�Χ
                int h = 0;//�������ĸ߶�
                int l = UnityEngine.Random.Range(5, 7);//��������һ��ı߳�
                while (h < 3)
                {
                    //ÿɾ��һ�㣬��һ��ı߳��ͻ���С
                    for (int rx = h; rx < l - h; rx++)
                    {
                        for (int ry = h; ry < l - h; ry++)
                        {
                            //����ǵ�ͼ��ʱ,��Ϊ�����б���һ���㿪ʼ���Һ���ɾ�������Բ���Ҫ�ж�С�ڵ����
                            if (_x + rx > World_x - 1 || _z + ry > World_z - 1)
                            {
                                continue;
                            }
                            if (j - h <= 0)
                            {
                                break;
                            }
                            ChangeObjUnit(_x + rx, j - h, _z + ry, 0);

                            if (unit[_x + rx, j - h, _z + ry].unitType != UnitType.Null)
                            {
                            }
                        }
                    }
                    h++;
                }
                return true;
            }
        }
        return true;
    }
    /// <summary>
    /// ���ĵ�λʵ��
    /// </summary>
    public void ChangeObjUnit(int _x, int _y, int _z, int index)
    {
        if (_x <= 0 || _x >= World_x || _y <= 0 || _y >= 2 * World_y - 1 || _z <= 0 || _z >= World_z)
        {
            return;
        }
        //�����¼����ڵ���ש�����ر��ڵ�����
        unit[_x, _y, _z].DesUnit();
        unit[_x, _y, _z] = Instantiate(prefab[index], this.transform);
        unit[_x, _y, _z].Posion = new Vector3(_x, _y, _z);
        MapMsg[_x * 2 * World_y * World_z + _y * World_z + _z] = index;
    }
    /// <summary>
    /// ͬ��ʱ���õ�ɾ�ķ���
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_z"></param>
    /// <param name="index"></param>
    public void SyncObjUnit(int _x, int _y, int _z, int index)
    {
        if (_x <= 0 || _x >= World_x || _y <= 0 || _y >= 2 * World_y - 1 || _z <= 0 || _z >= World_z)
        {
            return;
        }
        //�����¼����ڵ���ש�����ر��ڵ�����
        unit[_x, _y, _z].DesUnit();
        unit[_x, _y, _z] = Instantiate(prefab[index], this.transform);
        unit[_x, _y, _z].Posion = new Vector3(_x, _y, _z);
    }
    /// <summary>
    /// ͬ������
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="mapMsg"></param>
    public void SyncWorld(int x, int y, int z, List<int> mapMsg)
    {

        World_x = x;
        World_y = y;
        World_z = z;
        unit = new UnitObjBase[World_x, 2 * World_y, World_z];
        MapMsg = mapMsg;

        StartCoroutine("IESyncWorld", new Vector3(x, y, z));
    }
    public System.Collections.IEnumerator IESyncWorld(Vector3 vector3)
    {
        float maxValue = 2 * vector3.x * 2 * vector3.y * vector3.z + 10;
        LoadGame(maxValue, null);
        for (int i = 0; i < World_x; i++)
        {
            for (int j = 0; j < 2 * World_y; j++)
            {
                for (int k = 0; k < World_z; k++)
                {
                    unit[i, j, k] = Instantiate(prefab[0], transform);
                    unit[i, j, k].Posion = new Vector3(i, j, k);
                }
                loadPanel.CurrentFinishExtent += World_z - 1;
            }
            yield return new WaitForSeconds(.1f);
        }
        for (int j = 0; j < 2 * World_y; j++)
        {
            for (int i = 0; i < World_x; i++)
            {
                for (int k = 0; k < World_z; k++)
                {
                    SyncObjUnit(i, j, k, MapMsg[i * 2 * World_y * World_z + j * World_z + k]);
                }
                loadPanel.CurrentFinishExtent += World_z - 1;
            }
            yield return new WaitForSeconds(.1f);
        }
        loadPanel.CurrentFinishExtent = maxValue;
        Debug.Log("mapͬ�����");
    }
    /// <summary>
    /// ��������
    /// </summary>
    public void InitWorld(int x, int y, int z)
    {
        StartCoroutine("IEInitWorld", new Vector3(x, y, z));
    }
    public System.Collections.IEnumerator IEInitWorld(Vector3 vector3)
    {
        float maxValue = 10 + vector3.x * vector3.y + 10 + 10;
        LoadGame(maxValue, null);
        World_x = (int)vector3.x;
        World_y = (int)vector3.y;
        World_z = (int)vector3.z;
        unit = new UnitObjBase[World_x, 2 * World_y, World_z];
        MapMsg = new List<int>();
        for (int i = 0; i < World_x; i++)
        {
            for (int j = 0; j < 2 * World_y; j++)
            {
                for (int k = 0; k < World_z; k++)
                {
                    MapMsg.Add(0);
                    unit[i, j, k] = Instantiate(prefab[0], transform);
                    unit[i, j, k].Posion = new Vector3(i, j, k);
                }
            }
            yield return new WaitForSeconds(.1f);
        }
        loadPanel.CurrentFinishExtent = 10;
        //�������ڵķ�Χ�ڵ�Сһ�ŷ�Χ������block��ֹ�����±�Խ��
        for (int i = 1; i < World_x - 1; i++)
        {
            for (int j = 1; j < World_y - 1; j++)
            {
                for (int k = 1; k < World_z - 1; k++)
                {
                    ChangeObjUnit(i, j, k, 1);
                }
                loadPanel.CurrentFinishExtent += 1;
            }
            yield return new WaitForSeconds(.1f);
        }
        //��ʼ�����絥λ�ռ�  

        //�����������
        CreatingTerrain();
        loadPanel.CurrentFinishExtent += 10;
        yield return new WaitForSeconds(2f);
        CreateEarth();
        loadPanel.CurrentFinishExtent = maxValue;

        Debug.Log("��ͼ�������");
        CreateWorldInformation createWorldInformation = new CreateWorldInformation();
        createWorldInformation.MapMsg = MapMsg;
        Debug.Log("SyncMapSize:" + MapMsg.Count);

        createWorldInformation.x = World_x;
        createWorldInformation.y = World_y;
        createWorldInformation.z = World_z;
        createWorldInformation.roomId = �������.Id;
        NetManager.Send(createWorldInformation);
    }
    public void LoadGame(float maxValue, Action endAction)
    {
        loadPanel.Init(maxValue, GameManager.ins.InitGame);
    }
    public void ClearGame()
    {
        //  �ص�room��� 
        isGameTime = false;
        for (int i = 0; i < mainGame.transform.childCount; i++)
        {
            Destroy(mainGame.transform.GetChild(0).gameObject);
        }
        foreach (Transform child in mainGame.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < mainGame.bag.itemListParent.childCount; i++)
        {
            Destroy(mainGame.bag.itemListParent.GetChild(0).gameObject);
        }
        foreach (Transform child in mainGame.bag.itemListParent.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < ����.childCount; i++)
        {
            Destroy(����.GetChild(0).gameObject);
        }
        foreach (Transform child in ����.transform)
        {
            Destroy(child.gameObject);
        }
        �������.ins.ChangeUI();
    }
}
