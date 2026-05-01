import { onMounted, onUnmounted, ref, watch, type Ref } from 'vue'

export function useElementSize(target: Ref<HTMLElement | null>) {
  const width = ref(0)
  const height = ref(0)
  let observer: ResizeObserver | null = null

  function observe(el: HTMLElement) {
    observer = new ResizeObserver((entries) => {
      const entry = entries[0]
      if (!entry) return
      width.value = entry.contentRect.width
      height.value = entry.contentRect.height
    })
    observer.observe(el)
  }

  onMounted(() => {
    if (target.value) observe(target.value)
  })

  watch(target, (el, _, onCleanup) => {
    observer?.disconnect()
    observer = null
    if (el) observe(el)
    onCleanup(() => {
      observer?.disconnect()
      observer = null
    })
  })

  onUnmounted(() => {
    observer?.disconnect()
    observer = null
  })

  return { width, height }
}
