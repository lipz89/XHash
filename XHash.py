#!/usr/bin/python
# -*- coding: ascii -*-

from pbkdf2 import PBKDF2, b
from struct import pack
from random import randint
from base64 import b64encode
from base64 import b64decode

import sys

def create(input):
    saltBytes=b("").join([pack("@H", randint(0, 0xffff)) for i in range(12)])
    rawhash = PBKDF2(input, saltBytes, 5000).read(24)
    hash=b64encode(rawhash).decode('us-ascii')	
    saltB64=b64encode(saltBytes)
    salt=saltB64.decode('us-ascii')
    result=salt+hash
    return result

def validate(input,correctHash):
	salt=correctHash[:32]
	hash=correctHash[32:]
	rawhash = PBKDF2(input, b64decode(salt), 5000).read(24)
	hash1=b64encode(rawhash).decode('us-ascii')
	return slowEquals(hash,hash1)

def slowEquals(a, b):
	ba=b64decode(a)
	bb=b64decode(b)
	diff = len(ba) ^ len(bb);
	for i in range(len(ba)):
		diff=diff|(ba[i]^bb[i])
	return diff == 0

if __name__=='__main__':
    input="123456"
    print(input)	
    hash=create(input)
    print(hash)	
    valid=validate(input,hash)
    print(valid)
    input2="1234560"
    print(input2)	
    hash2=create(input2)
    print(hash2)	
    valid=validate(input2,hash2)
    print(valid)
    print("print false:")
    valid=validate(input,hash2)
    print(valid)
    valid=validate(input2,hash)
    print(valid)

