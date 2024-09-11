using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;
using UnityEngine.UI;

public class IngameUIManager : MonoBehaviour
{
    public static IngameUIManager Instance;

    [SerializeField]
    private IngameIntroUI ingameIntroUI;
    public IngameIntroUI IngameIntroUI {  get { return ingameIntroUI; } }

    [SerializeField]
    private KillButtonUI killButtonUI;
    public KillButtonUI KillButtonUI { get {return killButtonUI;} }

    [SerializeField]
    private KillUI killUI;
    public KillUI KillUI { get { return killUI; } }

    [SerializeField]
    private ReportButtonUI reportButtonUI;
    public ReportButtonUI ReportButtonUI { get { return reportButtonUI; } }

    [SerializeField]
    private ReportUI reportUI;
    public ReportUI ReportUI { get { return reportUI; } }

    [SerializeField]
    private MeetingUI meetingUI;
    public MeetingUI MeetingUI { get { return meetingUI; } }

    [SerializeField]
    private EjectionUI ejectionUI;
    public EjectionUI EjectionUI { get { return ejectionUI; } }

    [SerializeField]
    private OuttroUI outtroUI;
    public OuttroUI OuttroUI { get { return outtroUI; } }

    [SerializeField]
    private FixWiringTaskUIManager fixWiringTaskUI;
    public FixWiringTaskUIManager FixWiringTaskUI { get { return fixWiringTaskUI; } }

    [SerializeField]
    private Button useButton;

    [SerializeField]
    private Sprite originUseButtonSprite;

    private void Init()
    {
        IngameIntroUI.gameObject.SetActive(true);
        killUI.gameObject.SetActive(false);
        MeetingUI.gameObject.SetActive(false);
    }

    private void Awake()
    {
        Instance = this;
        Init();
    }

    public void SetUseButton(Sprite sprite, UnityAction action)
    {
        useButton.image.sprite = sprite;
        useButton.onClick.AddListener(action);
        useButton.interactable = true;
    }

    public void UnSetUseButton()
    {
        useButton.image.sprite = originUseButtonSprite;
        useButton.onClick.RemoveAllListeners();
        useButton.interactable = false;
    }

}
