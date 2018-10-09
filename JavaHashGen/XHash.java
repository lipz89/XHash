package JavaHashGen;

/** 
 * Password Hashing With PBKDF2 (http://crackstation.net/hashing-security.htm).
 * Copyright (c) 2013, Taylor Hornby
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without 
 * modification, are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, 
 * this list of conditions and the following disclaimer.
 *
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 * this list of conditions and the following disclaimer in the documentation 
 * and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE 
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
 * POSSIBILITY OF SUCH DAMAGE.
 */

import java.security.SecureRandom;

import javax.crypto.spec.DESKeySpec;
import javax.crypto.spec.PBEKeySpec;
import javax.crypto.Cipher;
import javax.crypto.SecretKeyFactory;

import com.sun.org.apache.xerces.internal.impl.dv.util.Base64;

import java.math.BigInteger;
import java.security.NoSuchAlgorithmException;
import java.security.spec.AlgorithmParameterSpec;
import java.security.spec.InvalidKeySpecException;

/**
 * PBKDF2 salted password hashing. Author: havoc AT defuse.ca www:
 * http://crackstation.net/hashing-security.htm
 */
public class XHash {
	private static final String PBKDF2_ALGORITHM = "PBKDF2WithHmacSHA1";

	// The following constants may be changed without breaking existing hashes.
	private static final int SALT_BYTE_SIZE = 24;
	private static final int HASH_BYTE_SIZE = 24;
	private static final int PBKDF2_ITERATIONS = 5000;

	private static final int SALT_LENGTH = 32;
	private static final int PBKDF2_LENGTH = 32;

	/**
	 * Returns a salted PBKDF2 hash of the password.
	 * 
	 * @param password
	 *            the password to hash
	 * @return a salted PBKDF2 hash of the password
	 */
	public static String create(String input) 
		throws NoSuchAlgorithmException, InvalidKeySpecException 
	{
		if(input==null||input.trim().equals(""))
			return null;
		return create((input).toCharArray());
	}

	/**
	 * Returns a salted PBKDF2 hash of the password.
	 * 
	 * @param password
	 *            the password to hash
	 * @return a salted PBKDF2 hash of the password
	 */
	private static String create(char[] password) 
		throws NoSuchAlgorithmException, InvalidKeySpecException 
	{
		// Generate a random salt
		SecureRandom random = new SecureRandom();
		byte[] salt = new byte[SALT_BYTE_SIZE];
		random.nextBytes(salt);

		// Hash the password
		byte[] hash = pbkdf2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);
		return Base64.encode(salt) + Base64.encode(hash);
	}
	
	/**
	 * Validates a password given a hash of the correct one.
	 * 
	 * @param input
	 *            The password to check
	 * @param correctHash
	 *            A hash of the correct password
	 * @return a salted PBKDF2 hash of the password
	 */
	public static boolean validate(String input, String correctHash)
		throws NoSuchAlgorithmException, InvalidKeySpecException 
	{
		if(input==null||input.trim().equals(""))
			return false;
		if(correctHash==null||correctHash.trim().equals(""))
			return false;
		return validatePassword(input.toCharArray(), correctHash);
	}

	/**
     * Validates a password using a hash.
     *
     * @param   password        the password to check
     * @param   correctHash     the hash of the valid password
     * @return                  true if the password is correct, false if not
     */
    private static boolean validatePassword(char[] password, String correctHash)
        throws NoSuchAlgorithmException, InvalidKeySpecException
    {
        // Decode the hash into its parameters
        byte[] salt = Base64.decode(correctHash.substring(0,SALT_LENGTH));
        byte[] hash = Base64.decode(correctHash.substring(SALT_LENGTH));
        // Compute the hash of the provided password, using the same salt, 
        // iteration count, and hash length
        byte[] testHash = pbkdf2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);
        // Compare the hashes in constant time. The password is correct if
        // both hashes match.
        return slowEquals(hash, testHash);
    }

	/**
	 * Computes the PBKDF2 hash of a password.
	 * 
	 * @param password
	 *            the password to hash.
	 * @param salt
	 *            the salt
	 * @param iterations
	 *            the iteration count (slowness factor)
	 * @param bytes
	 *            the length of the hash to compute in bytes
	 * @return the PBDKF2 hash of the password
	 */
	private static byte[] pbkdf2(char[] password, byte[] salt, int iterations, int bytes) 
		throws NoSuchAlgorithmException, InvalidKeySpecException 
	{
		PBEKeySpec spec = new PBEKeySpec(password, salt, iterations, bytes * 8);
		SecretKeyFactory skf = SecretKeyFactory.getInstance(PBKDF2_ALGORITHM);
		return skf.generateSecret(spec).getEncoded();
	}

	/**
     * Compares two byte arrays in length-constant time. This comparison method
     * is used so that password hashes cannot be extracted from an on-line 
     * system using a timing attack and then attacked off-line.
     * 
     * @param   a       the first byte array
     * @param   b       the second byte array 
     * @return          true if both byte arrays are the same, false if not
     */
    private static boolean slowEquals(byte[] a, byte[] b)
    {
        int diff = a.length ^ b.length;
        for(int i = 0; i < a.length && i < b.length; i++)
            diff |= a[i] ^ b[i];
        return diff == 0;
    }

	///**
	// * Tests the basic functionality of the PasswordHash class
	// * 
	// * @param args
	// *            ignored
	// */
	//public static void main(String[] args) {
	//	 try {
	//		 String tick="123456";
	//		 System.out.println(create(tick));	
	//	 } catch (Exception ex) {
	//		 System.out.println("ERROR: " + ex);
	//	 }
	// }
}