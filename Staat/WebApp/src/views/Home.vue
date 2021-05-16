<template>
  <div class="home">
    <q-page class="flex flex-center row">
      <q-card class="col-8">
        <q-card-section>
          <q-list>
            <q-expansion-item>
              <template v-slot:header>
                <q-item-section avatar>
                  <q-avatar icon="mdi-home" color="primary" text-color="white" />
                </q-item-section>
                <q-item-section>
                  Internal Sites
                </q-item-section>
                <q-item-section side>
                  <div class="row items-center">
                    <q-icon name="mdi-circle" color="red" size="24px">
                      <q-tooltip>
                        Test
                      </q-tooltip>
                    </q-icon>
                  </div>
                </q-item-section>
              </template>
              <q-card>
                <q-card-section>
                  Lorem ipsum dolor sit amet, consectetur adipisicing elit. Quidem, eius reprehenderit eos corrupti
                  commodi magni quaerat ex numquam, dolorum officiis modi facere maiores architecto suscipit iste
                  eveniet doloribus ullam aliquid.
                </q-card-section>
              </q-card>
            </q-expansion-item>
          </q-list>
        </q-card-section>
      </q-card>
    </q-page>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'Home'
});
</script>
