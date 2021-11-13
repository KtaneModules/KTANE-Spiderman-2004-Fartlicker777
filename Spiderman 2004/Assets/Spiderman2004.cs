using System.Collections;
using UnityEngine;

public class Spiderman2004 : MonoBehaviour {

   public KMBombInfo Bomb;
   private KMAudio.KMAudioRef SoundIThink;
   public KMAudio Audio;

   public KMSelectable mod;

   public KMSelectable Beeg;

   int clicks = 0;
   bool focused = false;
   bool highlighted = false;

   static int ModuleIdCounter = 1;
   int ModuleId;

   void Awake () {
      ModuleId = ModuleIdCounter++;

      Beeg.OnInteract += delegate () { BeegPress(); return false; };

      mod.OnHighlight += delegate () { MusicStarter(); highlighted = true; };
      mod.OnHighlightEnded += delegate () { MusicEnder(); highlighted = false; };
      mod.OnFocus += delegate () { focused = true; };
      mod.OnDefocus += delegate () { MusicEnder(); focused = false; };

      if (Application.isEditor) {
         focused = true;
      }
   }

   void MusicStarter () {
      if (SoundIThink == null) {
         SoundIThink = Audio.PlaySoundAtTransformWithRef("Pain", transform);
      }
   }

   void MusicEnder () {
      if (SoundIThink != null && !focused) {
         SoundIThink.StopSound();
         SoundIThink = null;
      }
   }

   void BeegPress () {
      Beeg.AddInteractionPunch();
      GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Beeg.transform);
      clicks++;
      Debug.LogFormat("[Spiderman 2004 #{0}] {1}", ModuleId, clicks);
      if (clicks == 2004) {
         GetComponent<KMBombModule>().HandlePass();
      }
   }

   void Start () {
      Debug.LogFormat("[Spiderman 2004 #{0}] Big Mistake.", ModuleId);
   }

   void Update () {
      if (SoundIThink != null && !focused && !highlighted) {
         SoundIThink.StopSound();
         SoundIThink = null;
      }
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} press to press the button once.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      Command = Command.Trim().ToUpper();
      yield return null;
      if (Command == "PRESS") {
         Beeg.OnInteract();
      }
      else {
         yield return "sendtochaterror Big mistake fucko.";
      }
   }

   IEnumerator TwitchHandleForcedSolve () {
      for (int i = 0; i < 2004 - clicks; i++) {
         Beeg.OnInteract();
         yield return new WaitForSeconds(.001f);
      }
   }
}
