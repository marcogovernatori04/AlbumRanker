<template>
  <div class="min-h-screen bg-neutral-950 text-white">
    <header class="border-b border-neutral-800 bg-neutral-900/80">
      <div class="mx-auto flex max-w-6xl items-center justify-between px-6 py-4">
        <nav class="flex items-center gap-4 text-sm">
          <RouterLink to="/albumes" class="text-neutral-300 hover:text-white">
            Álbumes
          </RouterLink>

          <RouterLink to="/comparativas" class="text-neutral-300 hover:text-white">
            Comparativas
          </RouterLink>
        </nav>
        <RouterLink to="/albumes" class="text-xl font-bold">
          Album Ranker
        </RouterLink>

      </div>
    </header>

    <main class="mx-auto max-w-6xl px-6 py-8">
      <RouterView />
    </main>
  </div>
</template>
