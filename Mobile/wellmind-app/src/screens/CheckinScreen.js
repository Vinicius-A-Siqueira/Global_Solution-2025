import * as ImagePicker from 'expo-image-picker';
import { useState } from 'react';
import { Alert, Button, Image, View } from 'react-native';
import { createEntry, detectEmotion } from '../api/wellbeingService';

export default function CheckinScreen() {
  const [photo, setPhoto] = useState(null);
  const takePhoto = async () => {
    const res = await ImagePicker.launchCameraAsync({ quality:0.6 });
    if (!res.cancelled) setPhoto(res.uri);
  };

  const send = async () => {
    if (!photo) return Alert.alert('Tire uma foto primeiro');
    try {
      const formData = new FormData();
      formData.append('file', { uri: photo, name: 'photo.jpg', type: 'image/jpeg' });
      const emo = await detectEmotion(formData);
      const payload = { emotion: emo.emocao || 'unknown', confidence: emo.confianca || 0, timestamp: new Date().toISOString() };
      await createEntry(payload);
      Alert.alert('Enviado', `Emoção: ${payload.emotion}`);
      setPhoto(null);
    } catch (e) {
      Alert.alert('Erro', 'Falha ao enviar');
    }
  };

  return (
    <View style={{flex:1,alignItems:'center',padding:16}}>
      {photo && <Image source={{uri:photo}} style={{width:240,height:320}} />}
      <Button title="Tirar Foto" onPress={takePhoto} />
      <Button title="Enviar Check‑in" onPress={send} />
    </View>
  );
}
