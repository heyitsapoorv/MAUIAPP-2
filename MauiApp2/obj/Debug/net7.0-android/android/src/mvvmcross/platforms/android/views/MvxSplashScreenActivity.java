package mvvmcross.platforms.android.views;


public abstract class MvxSplashScreenActivity
	extends mvvmcross.platforms.android.views.MvxActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onResume:()V:GetOnResumeHandler\n" +
			"n_onPause:()V:GetOnPauseHandler\n" +
			"";
		mono.android.Runtime.register ("MvvmCross.Platforms.Android.Views.MvxSplashScreenActivity, MvvmCross", MvxSplashScreenActivity.class, __md_methods);
	}


	public MvxSplashScreenActivity ()
	{
		super ();
		if (getClass () == MvxSplashScreenActivity.class) {
			mono.android.TypeManager.Activate ("MvvmCross.Platforms.Android.Views.MvxSplashScreenActivity, MvvmCross", "", this, new java.lang.Object[] {  });
		}
	}


	public MvxSplashScreenActivity (int p0)
	{
		super (p0);
		if (getClass () == MvxSplashScreenActivity.class) {
			mono.android.TypeManager.Activate ("MvvmCross.Platforms.Android.Views.MvxSplashScreenActivity, MvvmCross", "System.Int32, System.Private.CoreLib", this, new java.lang.Object[] { p0 });
		}
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onResume ()
	{
		n_onResume ();
	}

	private native void n_onResume ();


	public void onPause ()
	{
		n_onPause ();
	}

	private native void n_onPause ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
