import { useContext } from 'react';
import { Button, Text, View } from 'react-native';
import { AuthContext } from '../auth/AuthContext';
import { COMMIT_HASH } from '../config/commit';

export default function ProfileScreen() {
  const { user, logout } = useContext(AuthContext);
  return (
    <View style={{flex:1,padding:16}}>
      <Text>Usuário: {user?.email}</Text>
      <Text style={{marginTop:12}}>Commit: {COMMIT_HASH}</Text>
      <Button title="Logout" onPress={logout} />
    </View>
  );
}
