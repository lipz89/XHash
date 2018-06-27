#!/usr/bin/python
# -*- coding: ascii -*-

from pbkdf2 import PBKDF2, b
from struct import pack
from random import randint
from base64 import b64encode
from base64 import b64decode

import sys

def createHash(tick,publicKey):
    saltBytes=b("").join([pack("@H", randint(0, 0xffff)) for i in range(12)])
    saltB64=b64encode(saltBytes)
    salt=saltB64.decode('us-ascii')
    input=tick+publicKey
    rawhash = PBKDF2(input, b64decode(salt), 50).read(24)
    hash=b64encode(rawhash).decode('us-ascii')
    result=salt+hash
    return result
 

if __name__=='__main__':
    tick="123456"
    publicKey="0987654321"
    hash=createHash(tick,publicKey)
    print(hash)
