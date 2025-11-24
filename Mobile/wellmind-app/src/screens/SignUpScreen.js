import { createUserWithEmailAndPassword } from 'firebase/auth';
import { useState } from 'react';
import { Alert, Button, TextInput, View } from 'react-native';
import { auth } from '../auth/firebase';

export default function SignUpScreen({ navigation }) {
  const [email,setEmail]=useState('');
  const [pass,setPass]=useState('');

  const handleSignup = async () => {
    try {
      await createUserWithEmailAndPassword(auth, email, pass);
    } catch (e) {
      Alert.alert('Erro', e.message);
    }
  };

  return (
    <View style={{padding:16}}>
      <TextInput placeholder="Email" value={email} onChangeText={setEmail} autoCapitalize="none" />
      <TextInput placeholder="Senha" value={pass} onChangeText={setPass} secureTextEntry />
      <Button title="Registrar" onPress={handleSignup} />
    </View>
  );
}
