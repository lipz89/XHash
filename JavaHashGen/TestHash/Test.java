package TestHash;

import JavaHashGen.XHash;

public class Test{
	public static void main(String[] args) {
		try {
			String tick=null;
			String publicKey=null;
			if(args.length>1)
			{
				tick=args[0];
				publicKey=args[1];
			}
			System.out.println(XHash.createHash(tick,publicKey));	
		} catch (Exception ex) {
			System.out.println("ERROR: " + ex);
		}
	}
}