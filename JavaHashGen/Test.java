package JavaHashGen;

import JavaHashGen.XHash;

public class Test{
	public static void main(String[] args) {
		try {
			String input=null;
			if(args.length==1)
			{
				input=args[0];
				System.out.println(XHash.create(input));
			}
			else if(args.length==2)
			{
				input=args[0];
				String hash=args[1];
				System.out.println(XHash.validate(input,hash));
			}
		} catch (Exception ex) {
			System.out.println("ERROR: " + ex);
		}
	}
}