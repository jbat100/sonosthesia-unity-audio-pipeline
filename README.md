# sonosthesia-unity-audio-pipeline

Example unity integration of baked audio analysis generated using the python based CLI tool [sonosthesia-audio-pipeline](https://github.com/jbat100/sonosthesia-audio-pipeline).

## Dependencies

The following Unity packages are used

- [com.sonosthesia.signal](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.signal) the fundamental building block of the sonosthesia unity packages, provides access to streams of typed data using the observer pattern implemented by [UniRx](https://github.com/neuecc/UniRx)

- [com.sonosthesia.audio](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.audio) provides data containers for analysis data with no dependencies on underlying data formats or channels (msgpack, unity assets, websockets etc...). It allows analysis data to be sent to signals (see [com.sonosthesia.signal](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.signal))

- [com.sonosthesia.packaudio](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.packaudio) provides asset importers which allows audio analysis files to be decoded and loaded into structures which can be used by [com.sonosthesia.audio](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.audio)

- [com.sonosthesia.processing](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.processing) provides a number of processing algorithms used to smooth the audio analysis signals, removing their jittery / jerky character, making them better suited to drive natural looking visuals.


## Getting started

First you need to generate analysis files using the [sonosthesia-audio-pipeline](https://github.com/jbat100/sonosthesia-audio-pipeline). Refer to the project README for installation and usage.

Analysis `.xaa` files contain both continuous data computed on every audio frame with 512 sample hop size, and discreet data such as detected peaks. Future evolutions may contain additional data, such as bars, notes, mood, keys etc... An asset importer is provided along with custom timeline tracks which allow analysis data to be routed to signals.

- Create Unity timeline tracks available under `Sonosthsia.Audio/XAATrack` in the unity timeline. You can then drag and drop any .xaa file on to the track.

- Add one of the concrete `XAAHost` subclasses as component to a game object and bind it to the XAATrack. This is component to which the XAATrack sends analysis data to.

You can drag the `.xaa` file and the XAATrack, aligned with the associated audio file on an audio track. Note you can have multiple `.xaa` files for one audio track in the case where you ran the analysis on separated sources. The imported asset inpector gives information on the analysis track such as RMS magnitude ranges and number of extracted peaks.

<p align="center">
  <img alt="XAATrack" src="https://github.com/user-attachments/assets/9965bba9-6f3d-4e7b-8846-bf56e2aec7e2">
</p>  

Based on the host subclass you use:

### XAASignalHost

- Create a `ContinuousAnalysisSignal` and set it as target to the XAAHost. Use a `ContinuousAnalysisSplitter` to drive `FloatSignals`. These signals can then be used to drive any Sonosthesia target. 

- Create a `PeakAnalysisSignal` (note this is a discrete signal) and set it as target to the XAAHost. Use a `PeakAnalysisSplitter` to drive `PeakSignals`. These signals can then be used to drive instantiators or dispatchers.

### XAARelayHost

- Create an `XAARelay` scriptable object (under `Sonosthesia/Relay/XAARelay`) and set it as target the the XAARelayHost. 

- In any Unity scene, create a `XAARelayReceiver` component and set its source as the XAARelay you created.

- Target the outputs of the XAARelayReceiver to signals as described for XAASignalHost


## Configuration

The raw analysis data isn't great for driving visuals out of the box, a number of tools are available to address this. `XAAHost` subclasses point to a `XAAConfiguration` scriptable object which:

- remaps a given magnitude range (in dBs) to a normalized range (0, 1). Typically the dB range of interest is around (-35 to -5).   
- filters peaks under given magnitude (peak dB) and strength (normalized onset envelope peak), as the raw peak extraction can be overly sensitive.

For each of the different analysis frequency bands available (main, lows, mids, highs).

<p align="center">
  <img width="380" alt="XAAConfiguration" src="https://github.com/user-attachments/assets/6bf93f7e-6afd-40cd-9cd3-08baa60d11b9">
</p>  

## Processing and Filtering

The raw analysis signals can feel somewhat jerky and jittery. Using the signal [processing](https://github.com/jbat100/sonosthesia-unity-packages/tree/main/packages/com.sonosthesia.processing) facilities offered by sonosthesia, they can be smoothed in order to make them more suited to drive visuals. A specific filter factory is provided which chains a one euro filter, a soft landing filter (which prevents the signal droping above a given rate) and a range remaper. 

<p align="center">
  <img width="564" alt="SignalProcessing" src="https://github.com/user-attachments/assets/1e459cd1-18cc-4bcd-a8b3-6b55eeb08c6a">
</p>  

The `SignalHost` demo scenes shows a comparison of filtered vs unfiltered signals to illustrate the various algorithms.

## Demo scenes

A number of demo scenes are provided showing various ways of routing analysis signals. You can alter the timeline to use any audio file and associated `.xaa` files.

### SignalHost

The most basic setup. Using the custom timeline XAATrack to drive signals.

### RelayHost

Relays allow the signals to be routed through scriptable objects so that analysis signals generated by a Unity timeline track in one scene can drive signals in another scene.

## Additional targets

Sonosthesia provides many additional targets which can be driven using the signals generated by baked audio analyses. See additional demo projects such as [deform](https://github.com/jbat100/sonosthesia-unity-demo-deform) for details.
