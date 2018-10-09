#!/usr/bin/python
# -*- coding: ascii -*-

from pbkdf2 import PBKDF2, b
from struct import pack
from random import randint
from base64 import b64encode
from base64 import b64decode

import sys

def createHash(input):
    saltBytes=b("").join([pack("@H", randint(0, 0xffff)) for i in range(12)])
    saltB64=b64encode(saltBytes)
    salt=saltB64.decode('us-ascii')
    rawhash = PBKDF2(input, b64decode(salt), 5000).read(24)
    hash=b64encode(rawhash).decode('us-ascii')
    result=salt+hash
    return result
 

if __name__=='__main__':
    input="123456"
    hash=createHash(input)
    print(hash)
