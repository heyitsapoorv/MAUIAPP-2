package crc64db62d61d9af52c56;


public abstract class MvxFragment_1
	extends mvvmcross.platforms.android.views.fragments.MvxFragment
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MvvmCross.Platforms.Android.Views.Fragments.MvxFragment`1, MvvmCross", MvxFragment_1.class, __md_methods);
	}


	public MvxFragment_1 ()
	{
		super ();
		if (getClass () == MvxFragment_1.class) {
			mono.android.TypeManager.Activate ("MvvmCross.Platforms.Android.Views.Fragments.MvxFragment`1, MvvmCross", "", this, new java.lang.Object[] {  });
		}
	}


	public MvxFragment_1 (int p0)
	{
		super (p0);
		if (getClass () == MvxFragment_1.class) {
			mono.android.TypeManager.Activate ("MvvmCross.Platforms.Android.Views.Fragments.MvxFragment`1, MvvmCross", "System.Int32, System.Private.CoreLib", this, new java.lang.Object[] { p0 });
		}
	}

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
