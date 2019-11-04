﻿using System;
using FI.API.SignTool.Helpers;
using Shouldly;
using Xunit;

namespace FI.API.SignTool.Tests.Helpers
{
    public class CryptographyHelperTests
    {
        [Fact]
        public void GenerateDigitalSignature_generates_correct_known_signature()
        {
            // Act
            var result = CryptographyHelper.GenerateDigitalSignature(KnownData.KnownBody, KnownData.KnownPrivateKey);

            // Assert
            result.ShouldNotBeNull();
            result.Length.ShouldBeGreaterThan(0);
            Convert.ToBase64String(result)
                .ShouldBe(KnownData.KnownBodyDigitalSignature);
        }

        [Fact]
        public void VerifyDigitalSignature_Validates_known_signature_correctly()
        {
            // Act
            var result = CryptographyHelper.VerifyDigitalSignature(KnownData.KnownBodyDigitalSignature, KnownData.KnownBody, KnownData.KnownPublicKey);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void VerifyDigitalSignature_fails_to_validate_invalid_signature()
        {
            // Act
            var result = CryptographyHelper.VerifyDigitalSignature(Guid.NewGuid().ToString(), KnownData.KnownBody, KnownData.KnownPublicKey);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void HashBody_generates_correct_hash_for_known_body()
        {
            // Act
            var result = CryptographyHelper.HashBody(KnownData.KnownBody);

            // Assert
            result.ShouldBe(KnownData.KnownBodyHash);
        }

        [Fact]
        public void ImportPrivateKey_can_load_known_private_key()
        {
            // Act
            var result = CryptographyHelper.ImportPrivateKey(KnownData.KnownPrivateKey);

            // Assert
            result.ShouldNotBeNull();
            result.PublicOnly.ShouldBeFalse();
        }

        [Fact]
        public void ImportPrivateKey_fails_to_load_invalid_private_key()
        {
            // Act
            var privateKey = Guid.NewGuid().ToString();
            var result = Assert.Throws<ArgumentException>(
                () => CryptographyHelper.ImportPrivateKey(privateKey)
            );

            // Assert
            result.ShouldNotBeNull();
            result.ParamName.ShouldBe("privateKeyPEM");
        }

        [Fact]
        public void ImportPublicKey_can_load_known_public_key()
        {
            // Act
            var result = CryptographyHelper.ImportPublicKey(KnownData.KnownPublicKey);

            // Assert
            result.ShouldNotBeNull();
            result.PublicOnly.ShouldBeTrue();
        }

        [Fact]
        public void ImportPublicKey_fails_to_load_invalid_public_key()
        {
            // Act
            var privateKey = Guid.NewGuid().ToString();
            var result = Assert.Throws<ArgumentException>(
                () => CryptographyHelper.ImportPublicKey(privateKey)
            );

            // Assert
            result.ShouldNotBeNull();
            result.ParamName.ShouldBe("publicKeyPEM");
        }
    }
}
